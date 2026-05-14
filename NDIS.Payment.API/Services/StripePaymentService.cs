using Stripe;

namespace NDIS.Payment.API.Services
{
  public class StripePaymentService : IStripePaymentService
  {
    private readonly PaymentIntentService _paymentIntentService;

    public StripePaymentService()
    {
      _paymentIntentService = new PaymentIntentService();
    }

    public async Task<PaymentIntent> CreatePaymentIntentAsync(
            long amountInCents,
            string currency,
            string orderId,
            string idempotencyKey)
    {
      var options = new PaymentIntentCreateOptions
      {
        Amount = amountInCents,
        Currency = currency,
        AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
        {
          Enabled = true
        },
        Metadata = new Dictionary<string, string>
        {
          ["orderId"] = orderId
        }
      };

      var requestOptions = new RequestOptions
      {
        IdempotencyKey = idempotencyKey
      };

      return await _paymentIntentService.CreateAsync(options, requestOptions);
    }
  }
}
