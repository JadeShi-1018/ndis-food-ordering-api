using System.ComponentModel.DataAnnotations;

namespace NDIS.Payment.API.Domain
{
  public class ProcessedWebhookEvent
  {
    [Key]
    public string ProcessedWebhookEventId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Provider { get; set; } = "Stripe";

    // From Stripe webhook event.Id, for example evt_xxx
    [Required]
    public string EventId { get; set; } = null!;

    // For example: payment_intent.succeeded
    [Required]
    public string EventType { get; set; } = null!;

    [Required]
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
  }
}
