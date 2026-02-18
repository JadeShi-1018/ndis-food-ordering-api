namespace NDIS.Payment.API.Domain
{
    public class Payment
    {
        public string PaymentId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public float PaymentPrice { get; set; }
        public string UserRebateId { get; set; }
        public float RebateRate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        // Navigation properties
        public ICollection<PaymentEvent> PaymentEvents { get; set; } = new List<PaymentEvent>();
    }
}
