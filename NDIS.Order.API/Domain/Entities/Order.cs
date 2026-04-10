using NDIS.Order.API.Domain.Enums;

namespace NDIS.Order.API.Domain.Entities
{
    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
    public string CustomerContactNumber { get; set; } = null!;

    public string ProviderId { get; set; } = null!;
    public string ProviderServiceId { get; set; } = null!;
        public string ProviderServiceName { get; set; } = null!;
    public string ProviderPhoneNumber { get; set; } = null!;
    //public string ProviderServicePhoneNumber { get; set; } = null!;

    public string IdempotencyKey { get; set; } = null!;

    public string CategoryId { get; set; } = null!;
    public string CategoryName { get; set; } = null!;


    public string MenuId { get; set; } = null!;
        public string MenuName { get; set; } = null!;
    public string MenuDescription { get; set; } = null!;
   
    public string PeriodName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal OrderPrice { get; set; }

   

    public string? PaymentId { get; set; }





    
    public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   

        public OrderStatus OrderStatus { get; set; } = OrderStatus.PendingPayment;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderEvent> OrderEvents { get; set; } = new List<OrderEvent>();
  }
}
