using MassTransit;
using Microsoft.EntityFrameworkCore;
using NDIS.Contracts.Events;
using NDIS.Payment.API.Domain;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Enums;
using NDIS.Payment.API.Repositories;
using NDIS.Payment.API.Services.Outbox;
using Stripe;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;
//using NDIS.Payment.API.IntegrationEvents;
using NDIS.Payment.API.Data;
using System.Text.Json;

namespace NDIS.Payment.API.Services
{
  public class PaymentService : IPaymentService
  {
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IStripePaymentService _stripePaymentService;
    private readonly ApplicationDbContext _context;


    public PaymentService(
        IPaymentRepository paymentRepository,
        ILogger<PaymentService> logger,
        IPublishEndpoint publishEndpoint,
      IStripePaymentService stripePaymentService, ApplicationDbContext applicationDbContext)
    {
      _paymentRepository = paymentRepository;
      _logger = logger;
      _publishEndpoint = publishEndpoint;
      _stripePaymentService = stripePaymentService;
      _context = applicationDbContext;
    }

    public async Task<PaymentEntity> CreatePaymentForOrderAsync(OrderCreatedEvent message)
    {
      var existingPayment = await _paymentRepository.GetByOrderIdAsync(message.OrderId);

      if (existingPayment != null)
      {
        _logger.LogInformation(
            "Payment already exists for OrderId={OrderId}. PaymentId={PaymentId}",
            message.OrderId,
            existingPayment.PaymentId);

        return existingPayment;
      }

      var amountInCents = ToAmountInCents(message.OrderPrice);
      var currency = "aud";
      var paymentIdempotencyKey = $"payment-intent:{message.OrderId}";

      var paymentIntent = await _stripePaymentService.CreatePaymentIntentAsync(
          amountInCents,
          currency,
          message.OrderId,
          paymentIdempotencyKey);

      var payment = new PaymentEntity
      {
        OrderId = message.OrderId,
        UserId = message.UserId,
        CustomerName = message.CustomerName,

        ProviderId = message.ProviderId,
        ProviderServiceId = message.ProviderServiceId,
        ProviderServiceName = message.ProviderServiceName,

        CategoryId = message.CategoryId,
        CategoryName = message.CategoryName,

        MenuId = message.MenuId,
        MenuName = message.MenuName,
        PeriodName = message.PeriodName,

        Quantity = message.Quantity,
        UnitPrice = message.UnitPrice,
        PaymentPrice = message.OrderPrice,

        Currency = currency,
        AmountInCents = amountInCents,

        PaymentStatus = PaymentStatus.Pending,

        OrderIdempotencyKey = message.IdempotencyKey,
        PaymentIdempotencyKey = paymentIdempotencyKey,

        StripePaymentIntentId = paymentIntent.Id,
        StripeClientSecret = paymentIntent.ClientSecret,
        PaymentIntentCreatedAt = DateTime.UtcNow,

        OrderCreatedAt = message.CreatedAt,
        CreatedAt = DateTime.UtcNow
      };

      try
      {
        await _paymentRepository.AddAsync(payment);
        return payment;
      }
      catch (DbUpdateException ex)
      {
        _logger.LogWarning(
            ex,
            "Possible duplicate payment insert for OrderId={OrderId}. Trying to load existing payment.",
            message.OrderId);

        var savedPayment = await _paymentRepository.GetByOrderIdAsync(message.OrderId);

        if (savedPayment != null)
        {
          return savedPayment;
        }

        throw;
      }
    }

    private static long ToAmountInCents(decimal amount)
    {
      return (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
    }

    public async Task<PaymentByOrderResponseDto?> GetPaymentByOrderIdAsync(string orderId)
    {
      var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

      if (payment == null)
      {
        return null;
      }

      return new PaymentByOrderResponseDto
      {
        PaymentId = payment.PaymentId,
        OrderId = payment.OrderId,
        UserId = payment.UserId,

        PaymentPrice = payment.PaymentPrice,
        Currency = payment.Currency,
        AmountInCents = payment.AmountInCents,
        PaymentStatus = payment.PaymentStatus.ToString(),

        StripePaymentIntentId = payment.StripePaymentIntentId,
        StripeClientSecret = payment.StripeClientSecret,

        CustomerName = payment.CustomerName,
        ProviderServiceName = payment.ProviderServiceName,
        MenuName = payment.MenuName,
        PeriodName = payment.PeriodName,
        Quantity = payment.Quantity,

        CreatedAt = payment.CreatedAt
      };
    }

    public async Task HandlePaymentSucceededAsync(
    string stripeEventId,
    string stripePaymentIntentId)
    {
      await using var transaction = await _context.Database.BeginTransactionAsync();

      var alreadyProcessed = await _context.ProcessedWebhookEvents
          .AnyAsync(x => x.Provider == "Stripe" && x.EventId == stripeEventId);

      if (alreadyProcessed)
      {
        await transaction.CommitAsync();
        return;
      }

      _context.ProcessedWebhookEvents.Add(new ProcessedWebhookEvent
      {
        Provider = "Stripe",
        EventId = stripeEventId,
        EventType = "payment_intent.succeeded",
        ProcessedAt = DateTime.UtcNow
      });

      var payment = await _context.Payments
          .FirstOrDefaultAsync(x => x.StripePaymentIntentId == stripePaymentIntentId);

      if (payment == null)
      {
        throw new InvalidOperationException(
            $"Payment not found for StripePaymentIntentId={stripePaymentIntentId}");
      }

      if (payment.PaymentStatus != PaymentStatus.Succeeded)
      {
        payment.PaymentStatus = PaymentStatus.Succeeded;
        payment.PaidAt = DateTime.UtcNow;
        payment.UpdatedAt = DateTime.UtcNow;

        var integrationEvent = new PaymentSucceededEvent
        {
          PaymentId = payment.PaymentId,
          OrderId = payment.OrderId,
          UserId = payment.UserId,
          Amount = payment.PaymentPrice,
          Currency = payment.Currency,
          StripePaymentIntentId = payment.StripePaymentIntentId!
        };

        _context.OutboxMessages.Add(new OutboxMessage
        {
          EventType = integrationEvent.EventType,
          Payload = JsonSerializer.Serialize(integrationEvent),
          OccurredAt = DateTime.UtcNow
        });
      }

      await _context.SaveChangesAsync();
      await transaction.CommitAsync();
    }

  }
}
