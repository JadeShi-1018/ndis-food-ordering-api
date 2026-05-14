using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDIS.Contracts.Events
{
  public class PaymentSucceededEvent
  {
    public string EventId { get; set; } = Guid.NewGuid().ToString();

    public string EventType { get; set; } = "PaymentSucceeded";

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public string PaymentId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "aud";

    public string StripePaymentIntentId { get; set; } = null!;
  }
}
