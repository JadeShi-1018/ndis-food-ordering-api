using NDIS.Order.API.Domain.Enums;

namespace NDIS.Order.API.Domain.Entities
{
  public class OrderEvent
  {
    public string OrderEventId { get; set; } = Guid.NewGuid().ToString();

    public string OrderId { get; set; } = null!;

    public OrderEventType EventType { get; set; }

    public OrderEventStatus EventStatus { get; set; } = OrderEventStatus.Pending;

    public string Payload { get; set; } = null!;

    public int RetryCount { get; set; } = 0;

    public string? ErrorMessage { get; set; }

    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAt { get; set; }

    public Order Order { get; set; } = null!;
    public DateTime? NextRetryAt { get; set; }

    public DateTime? LockedAt { get; set; }
  }
}