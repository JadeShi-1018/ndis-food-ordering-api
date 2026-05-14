using NDIS.Contracts.Events;
using NDIS.Payment.API.Dtos;
using Stripe;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Services
{
  public interface IPaymentService
  {
    Task<PaymentEntity> CreatePaymentForOrderAsync(OrderCreatedEvent message);
    Task<PaymentByOrderResponseDto?> GetPaymentByOrderIdAsync(string orderId);
    Task HandlePaymentSucceededAsync(
    string stripeEventId,
    string stripePaymentIntentId);
  }
}
