using NDIS.Payment.API.Domain.StateMachines;
using NDIS.Payment.API.Enums;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace NDIS.Payment.API.Domain
{
  public class Payment
  {
    [Key]
    public string PaymentId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string OrderId { get; set; } = null!;

    public string? StripePaymentIntentId { get; set; }

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

    public void ChangeStatus(PaymentStatus newStatus)
    {
      if(!PaymentStatusStateMachine.CanTransition(this.PaymentStatus, newStatus))
      {
        throw new InvalidCastException($"Invalid status transaction: {this.PaymentStatus} -> {newStatus}");
      }

      this.PaymentStatus = newStatus;
      this.UpdatedAt = DateTime.UtcNow;
    }
  }
}