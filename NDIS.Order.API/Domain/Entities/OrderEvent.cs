namespace NDIS.Order.API.Domain.Entities
{
    public class OrderEvent
    {

        public string OrderEventId { get; set; }

        public string OrderId { get; set; }

        public string EventType { get; set; }

        public string EventStatus { get; set; }

        public DateTime EventTimeStamp { get; set; }

        public Order Order { get; set; }
    }
}
