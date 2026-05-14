using Stripe;

namespace NDIS.Payment.API.Services
{
  public interface IStripePaymentService
  {
    Task<PaymentIntent> CreatePaymentIntentAsync(
             long amountInCents,
             string currency,
             string orderId,
             string idempotencyKey);
  }
}
