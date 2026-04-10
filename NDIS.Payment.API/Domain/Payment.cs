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

    public string? PaymentMethod { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    [Required]
    public string IdempotencyKey { get; set; } = null!;

    [Required]
    public DateTime OrderCreatedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<PaymentEvent> PaymentEvents { get; set; } = new List<PaymentEvent>();
  }
}
