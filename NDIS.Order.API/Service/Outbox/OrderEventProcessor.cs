using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDIS.Contracts.Events;
using NDIS.Order.API.DataAccess;
using NDIS.Order.API.Domain.Enums;

namespace NDIS.Order.API.Services.Outbox
{
  public class OrderEventProcessor : BackgroundService
  {
    private readonly IServiceScopeFactory _scopeFactory;
  
    private readonly ILogger<OrderEventProcessor> _logger;

    public OrderEventProcessor(
        IServiceScopeFactory scopeFactory,
        
        ILogger<OrderEventProcessor> logger)
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
          using var scope = _scopeFactory.CreateScope();
          var db = scope.ServiceProvider.GetRequiredService<
            OrderDbContext>();
          var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

          var pendingEvents = await db.OrderEvents
              .Where(x => x.EventStatus == OrderEventStatus.Pending)
              .OrderBy(x => x.EventTimestamp)
              .Take(20)
              .ToListAsync(stoppingToken);

          foreach (var orderEvent in pendingEvents)
          {
            try
            {
              switch (orderEvent.EventType)
              {
                case OrderEventType.OrderCreated:
                  {
                    var payload = JsonSerializer.Deserialize<OrderCreatedEvent>(orderEvent.Payload);

                    if (payload == null)
                    {
                      throw new InvalidOperationException(
                          $"Failed to deserialize OrderCreatedEvent payload. OrderEventId={orderEvent.OrderEventId}");
                    }

                    _logger.LogInformation(
                        "Publishing OrderCreatedEvent. OrderEventId={OrderEventId}, OrderId={OrderId}",
                        orderEvent.OrderEventId,
                        payload.OrderId);

                    await publishEndpoint.Publish(payload, stoppingToken);
                    break;
                  }

                default:
                  throw new NotSupportedException(
                      $"Unsupported event type: {orderEvent.EventType}");
              }

              orderEvent.EventStatus = OrderEventStatus.Processed;
              orderEvent.ProcessedAt = DateTime.UtcNow;
              orderEvent.ErrorMessage = null;

              _logger.LogInformation(
                  "OrderEvent processed successfully. OrderEventId={OrderEventId}",
                  orderEvent.OrderEventId);
            }
            catch (Exception ex)
            {
              orderEvent.RetryCount += 1;
              orderEvent.ErrorMessage = ex.Message;
              orderEvent.EventStatus = orderEvent.RetryCount >= 5
                  ? OrderEventStatus.Failed
                  : OrderEventStatus.Pending;

              _logger.LogError(
                  ex,
                  "Failed to process OrderEvent. OrderEventId={OrderEventId}, RetryCount={RetryCount}",
                  orderEvent.OrderEventId,
                  orderEvent.RetryCount);
            }
          }

          await db.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "OrderEventProcessor loop failed.");
        }

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
      }
    }
  }
}