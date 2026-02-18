using System.ComponentModel.DataAnnotations.Schema;

namespace NDIS.Payment.API.Domain
{
    public class PaymentEvent
    {
        public string PaymentEventId { get; set; }

        [ForeignKey("Payment")]
        public string PaymentId { get; set; }
        public string EventType { get; set; }
        public string EventStatus { get; set; }
        public DateTime EventTimestamp { get; set; }
        // Navigation property
        public Payment Payment { get; set; }
    }
}
