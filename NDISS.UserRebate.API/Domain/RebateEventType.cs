using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NDISS.UserRebate.API.Domain
{
    public class RebateEventType
    {
        public string RebateEventTypeId { get; set; }
        public string RebateEventTypeName { get; set; }
        public ICollection<RebateEvent> RebateEvents { get; set; } = new List<RebateEvent>();

    }
}