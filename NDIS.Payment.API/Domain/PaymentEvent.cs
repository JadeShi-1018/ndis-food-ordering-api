using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDIS.Payment.API.Domain
{
    public class PaymentEvent
    {
    [Key]
    public string PaymentEventId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [ForeignKey(nameof(Payment))]
    public string PaymentId { get; set; } = null!;

    [Required]
    public string EventType { get; set; } = null!;

    [Required]
    public string EventStatus { get; set; } = null!;

    [Required]
    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Payment Payment { get; set; } = null!;
  }
}
