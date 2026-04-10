using MassTransit;
using NDIS.Contracts.Events;
using NDIS.Payment.API.Domain;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Enums;
using NDIS.Payment.API.Repositories;
using Stripe;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Services
{
  public class PaymentService : IPaymentService
  {
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    

    public PaymentService(
        IPaymentRepository paymentRepository,
        ILogger<PaymentService> logger,
        IPublishEndpoint publishEndpoint)
    {
      _paymentRepository = paymentRepository;
      _logger = logger;
      _publishEndpoint = publishEndpoint;
    }

    public async Task CreatePaymentFromOrderAsync(OrderCreatedEvent message)
    {
      var existingPayment = await _paymentRepository.GetByOrderIdAsync(message.OrderId);
      if (existingPayment != null)
      {
        _logger.LogWarning(
            "Payment already exists for OrderId: {OrderId}. Skipping duplicate consumption.",
            message.OrderId);
        return;
      }

      var payment = new PaymentEntity
      {
        PaymentId = Guid.NewGuid().ToString(),
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
        PaymentStatus = PaymentStatus.Pending,
        IdempotencyKey = message.IdempotencyKey,
        OrderCreatedAt = message.CreatedAt,
        CreatedAt = DateTime.UtcNow
      };

      await _paymentRepository.AddAsync(payment);

      var paymentEvent = new PaymentEvent
      {
        PaymentEventId = Guid.NewGuid().ToString(),
        PaymentId = payment.PaymentId,
        EventType = "PaymentCreated",
        EventStatus = "Completed",
        EventTimestamp = DateTime.UtcNow
      };

      await _paymentRepository.AddPaymentEventAsync(paymentEvent);
      await _paymentRepository.SaveChangesAsync();

      _logger.LogInformation(
          "Payment created successfully. PaymentId: {PaymentId}, OrderId: {OrderId}",
          payment.PaymentId,
          payment.OrderId);
    }

    public async Task<PaymentResponseDto?> GetByOrderIdAsync(string orderId)
    {
      var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

      if (payment == null) return null;

      return new PaymentResponseDto
      {
        PaymentId = payment.PaymentId,
        OrderId = payment.OrderId,
        PaymentStatus = payment.PaymentStatus.ToString(),
        PaymentPrice = payment.PaymentPrice,
        PaymentMethod = payment.PaymentMethod,
        CreatedAt = payment.CreatedAt
      };
    }

    public async Task<bool> PayAsync(PayOrderRequestDto request)
    {
      var payment = await _paymentRepository.GetByOrderIdAsync(request.OrderId);

      if (payment == null)
      {
        _logger.LogWarning("Payment not found for OrderId: {OrderId}", request.OrderId);
        return false;
      }

      if (payment.PaymentStatus == PaymentStatus.Success)
      {
        _logger.LogWarning("Payment already succeeded for OrderId: {OrderId}", request.OrderId);
        return true;
      }

      var paymentIntentService = new PaymentIntentService();

      var options = new PaymentIntentCreateOptions
      {
        Amount = (long)(payment.PaymentPrice * 100m),
        Currency = "aud",
        Confirm = true,
        PaymentMethod = "pm_card_visa",
        PaymentMethodTypes = new List<string> { "card" },
        Metadata = new Dictionary<string, string>
        {
            { "orderId", payment.OrderId },
            { "paymentId", payment.PaymentId }
        }
      };

      var intent = await paymentIntentService.CreateAsync(options);

      if (intent.Status != "succeeded")
      {
        _logger.LogWarning(
            "Stripe payment did not succeed. OrderId: {OrderId}, PaymentIntentId: {PaymentIntentId}, Status: {Status}",
            payment.OrderId,
            intent.Id,
            intent.Status);

        return false;
      }

      payment.PaymentMethod = request.PaymentMethod;
      payment.PaymentStatus = PaymentStatus.Success;
      payment.UpdatedAt = DateTime.UtcNow;

      var paymentEvent = new PaymentEvent
      {
        PaymentEventId = Guid.NewGuid().ToString(),
        PaymentId = payment.PaymentId,
        EventType = "PaymentSucceeded",
        EventStatus = "Completed",
        EventTimestamp = DateTime.UtcNow
      };

      await _paymentRepository.AddPaymentEventAsync(paymentEvent);
      await _paymentRepository.SaveChangesAsync();

      await _publishEndpoint.Publish(new PaymentSucceededEvent
      {
        OrderId = payment.OrderId,
        PaymentId = payment.PaymentId,
        Amount = payment.PaymentPrice,
        PaidAt = DateTime.UtcNow
      });

      _logger.LogInformation(
          "Stripe test payment succeeded. OrderId: {OrderId}, PaymentId: {PaymentId}, PaymentIntentId: {PaymentIntentId}",
          payment.OrderId,
          payment.PaymentId,
          intent.Id);

      return true;
    }
  }
}
