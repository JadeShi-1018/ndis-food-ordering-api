namespace NDIS.Payment.API.Dtos
{
  public class PaymentByOrderResponseDto
  {

 
      public string PaymentId { get; set; } = null!;

      public string OrderId { get; set; } = null!;

      public string UserId { get; set; } = null!;

      public decimal PaymentPrice { get; set; }

      public string Currency { get; set; } = "aud";

      public long AmountInCents { get; set; }

      public string PaymentStatus { get; set; } = null!;

      public string? StripePaymentIntentId { get; set; }

      public string? StripeClientSecret { get; set; }

      public string CustomerName { get; set; } = null!;

      public string ProviderServiceName { get; set; } = null!;

      public string MenuName { get; set; } = null!;

      public string PeriodName { get; set; } = null!;

      public int Quantity { get; set; }

      public DateTime CreatedAt { get; set; }
   
}
}
