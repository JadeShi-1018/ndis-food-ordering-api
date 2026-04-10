using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDIS.Contracts.Events
{
    public class PaymentSucceededEvent
    {
    public string OrderId { get; set; } = null!;
    public string PaymentId { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
  }
}
