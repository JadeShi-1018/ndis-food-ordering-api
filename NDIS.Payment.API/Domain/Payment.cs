using NDIS.Payment.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace NDIS.Payment.API.Domain
{
    public class Payment
    {
    [Key]
    public string PaymentId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string OrderId { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string CustomerName { get; set; } = null!;

    [Required]
    public string ProviderId { get; set; } = null!;

    [Required]
    public string ProviderServiceId { get; set; } = null!;

    [Required]
    public string ProviderServiceName { get; set; } = null!;

    [Required]
    public string CategoryId { get; set; } = null!;

    [Required]
    public string CategoryName { get; set; } = null!;

    [Required]
    public string MenuId { get; set; } = null!;

    [Required]
    public string MenuName { get; set; } = null!;

    [Required]
    public string PeriodName { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public decimal PaymentPrice { get; set; }



    [Required]
    public string Currency { get; set; } = "aud";

    [Required]
    public long AmountInCents { get; set; }

    public string? PaymentMethod { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    // Used when calling Stripe to create PaymentIntent.
    // Recommended value: payment-intent:{OrderId}
    [Required]
    public string PaymentIdempotencyKey { get; set; } = null!;
    public string OrderIdempotencyKey { get; set; } = null!;

    public string? StripePaymentIntentId { get; set; }

    public string? StripeClientSecret { get; set; }

    public DateTime? PaymentIntentCreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public string? FailureReason { get; set; }




    [Required]
    public DateTime OrderCreatedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }



  }
}
