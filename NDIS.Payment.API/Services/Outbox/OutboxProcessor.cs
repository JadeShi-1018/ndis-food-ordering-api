using MassTransit;
using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Data;
using NDIS.Contracts.Events;
using System.Text.Json;

namespace NDIS.Payment.API.Services.Outbox
{
  public class OutboxProcessor : BackgroundService
  {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessor> logger)
    {
      _scopeFactory = scopeFactory;
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          await ProcessOutboxMessagesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error occurred while processing payment outbox messages.");
        }

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
      }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
      using var scope = _scopeFactory.CreateScope();

      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

      var messages = await dbContext.OutboxMessages
          .Where(x => x.ProcessedAt == null)
          .OrderBy(x => x.OccurredAt)
          .Take(20)
          .ToListAsync(cancellationToken);

      foreach (var message in messages)
      {
        try
        {
          if (message.EventType == "PaymentSucceeded")
          {
            var integrationEvent = JsonSerializer.Deserialize<PaymentSucceededEvent>(message.Payload);

            if (integrationEvent == null)
            {
              throw new InvalidOperationException(
                  $"Failed to deserialize PaymentSucceededEvent. OutboxMessageId={message.OutboxMessageId}");
            }

            await publishEndpoint.Publish(integrationEvent, cancellationToken);

            message.ProcessedAt = DateTime.UtcNow;
            message.Error = null;

            _logger.LogInformation(
                "Published PaymentSucceededEvent. OutboxMessageId={OutboxMessageId}, EventId={EventId}, OrderId={OrderId}",
                message.OutboxMessageId,
                integrationEvent.EventId,
                integrationEvent.OrderId);
          }
          else
          {
            _logger.LogWarning(
                "Unknown outbox event type. OutboxMessageId={OutboxMessageId}, EventType={EventType}",
                message.OutboxMessageId,
                message.EventType);

            message.RetryCount++;
            message.Error = $"Unknown EventType: {message.EventType}";
          }
        }
        catch (Exception ex)
        {
          message.RetryCount++;
          message.Error = ex.Message;

          _logger.LogError(
              ex,
              "Failed to publish outbox message. OutboxMessageId={OutboxMessageId}, EventType={EventType}, RetryCount={RetryCount}",
              message.OutboxMessageId,
              message.EventType,
              message.RetryCount);
        }
      }

      if (messages.Count > 0)
      {
        await dbContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
