using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDIS.Contracts.Events
{
    public class OrderCreatedEvent
    {
      public string OrderId { get; set; } = null!;
      public string UserId { get; set; } = null!;

      public string CustomerName { get; set; } = null!;

      public string ProviderId { get; set; } = null!;
      public string ProviderServiceId { get; set; } = null!;
      public string ProviderServiceName { get; set; } = null!;

      public string CategoryId { get; set; } = null!;
      public string CategoryName { get; set; } = null!;

      public string MenuId { get; set; } = null!;
      public string MenuName { get; set; } = null!;

      public string PeriodName { get; set; } = null!;
      public int Quantity { get; set; }
      public decimal UnitPrice { get; set; }
      public decimal OrderPrice { get; set; }

      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }

      public string IdempotencyKey { get; set; } = null!;
      public DateTime CreatedAt { get; set; }

    }



  }



