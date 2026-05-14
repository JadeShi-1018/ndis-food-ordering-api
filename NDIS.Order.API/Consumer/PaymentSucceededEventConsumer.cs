using MassTransit;
using NDIS.Contracts.Events;
using NDIS.Order.API.Services;

namespace NDIS.Order.API.Consumer
{
 
    public class PaymentSucceededEventConsumer : IConsumer<PaymentSucceededEvent>
    {
      private readonly IOrderService _orderService;
      private readonly ILogger<PaymentSucceededEventConsumer> _logger;

      public PaymentSucceededEventConsumer(
          IOrderService orderService,
          ILogger<PaymentSucceededEventConsumer> logger)
      {
        _orderService = orderService;
        _logger = logger;
      }

      public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
      {
        var message = context.Message;

        _logger.LogInformation(
            "Received PaymentSucceededEvent. EventId={EventId}, OrderId={OrderId}, PaymentId={PaymentId}, Amount={Amount} {Currency}",
            message.EventId,
            message.OrderId,
            message.PaymentId,
            message.Amount,
            message.Currency);

        await _orderService.MarkOrderAsPaidAsync(message);
      }
    }
}
