namespace NDIS.Order.API.Dtos
{
 
    public class OrderResponseDto
    {
      public string OrderId { get; set; } = null!;

      public string UserId { get; set; } = null!;
      public string CustomerName { get; set; } = null!;

      public string ProviderId { get; set; } = null!;
      public string ProviderName { get; set; } = null!;

      //public string ProviderServicePlanId { get; set; } = null!;
      //public string ProviderServicePlanName { get; set; } = null!;

      public string MenuId { get; set; } = null!;
      public string MenuName { get; set; } = null!;
      public string PeriodName { get; set; } = null!;

      public int Quantity { get; set; }
      public decimal UnitPrice { get; set; }
      public decimal OrderPrice { get; set; }

      public string? PaymentId { get; set; }

      public string DeliveryAddress { get; set; } = null!;
      public string CustomerContactNumber { get; set; } = null!;
      //public string ProviderContactNumber { get; set; } = null!;

      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }

      public string OrderStatus { get; set; } = null!;

      public DateTime CreatedAt { get; set; }
      public DateTime UpdatedAt { get; set; }
    }
  }

