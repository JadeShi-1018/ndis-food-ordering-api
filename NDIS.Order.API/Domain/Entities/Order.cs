namespace NDIS.Order.API.Domain.Entities
{
    public class Order
    {
        public string OrderId { get; set; }

        public string UserId { get; set; }

        public string CustomerName { get; set; }

        public string ProviderId { get; set; }

        public string ProviderName { get; set; }

        public string ProviderServicePlanId { get; set; }


        public string ProviderServicePlanName { get; set; }

        public float OrderPrice { get; set; }

        public string PaymentId { get; set; }

        public string DeliveryAddress { get; set; }

        public string CustomerContactNumber { get; set; }

        public string ProviderContactNumber { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public DateTime UpdateAt { get; set; } = DateTime.Now;

        public ICollection<OrderEvent> OrderEvents { get; set; }
    }
}
