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
    public decimal Amount { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public string? PaymentMethod { get; set; }

    [Required]
    public string IdempotencyKey { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
  }
}