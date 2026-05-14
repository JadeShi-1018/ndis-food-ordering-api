using MassTransit;
using NDIS.Contracts.Events;
using NDIS.Payment.API.Domain;
using NDIS.Payment.API.Services;

namespace NDIS.Payment.API.Consumers
{
  public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
  {
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IPaymentService _paymentService;

    public OrderCreatedConsumer(
        ILogger<OrderCreatedConsumer> logger,
        IPaymentService paymentService)
    {
      _logger = logger;
      _paymentService = paymentService;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
      var message = context.Message;

      _logger.LogInformation(
          "Received OrderCreatedEvent. OrderId: {OrderId}, UserId: {UserId}, IdempotencyKey: {IdempotencyKey}",
          message.OrderId,
          message.UserId,
          message.IdempotencyKey);

     var payment = await _paymentService.CreatePaymentForOrderAsync(message);
      _logger.LogInformation(
       "Payment initialized. OrderId: {OrderId}, PaymentId: {PaymentId}, StripePaymentIntentId: {StripePaymentIntentId}, Status: {PaymentStatus}",
       payment.OrderId,
      payment.PaymentId,
       payment.StripePaymentIntentId,
       payment.PaymentStatus);
    }
  }
}