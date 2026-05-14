namespace NDIS.Payment.API.Enums
{
  public enum PaymentStatus
  {
  
    Pending = 1,      // Payment record created, waiting for user to pay
    Succeeded = 2,    // Stripe confirmed payment_intent.succeeded
    Failed = 3        // Stripe confirmed payment_intent.payment_failed
  
}
}
