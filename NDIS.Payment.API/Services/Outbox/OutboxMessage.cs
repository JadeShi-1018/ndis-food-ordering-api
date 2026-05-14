using Stripe;

namespace NDIS.Payment.API.Services.Outbox
{
  public class OutboxMessage
  {
    public string OutboxMessageId { get; set; } = Guid.NewGuid().ToString();

    public string EventType { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAt { get; set; }

    public string? Error { get; set; }

    public int RetryCount { get; set; }
  }
}
