using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NDISS.UserRebate.API.Enums;

namespace NDISS.UserRebate.API.Domain
{
    public class RebateEvent
    {
        [Key]
        public string RebateEventId { get; set; }

        public ICollection<RebateRetrySchedule> RebateRetrySchedules { get; set; } = new List<RebateRetrySchedule>();

        [ForeignKey("RebateEventType")]
        public string RebateEventTypeId { get; set; }

        [ForeignKey("UserRebate")]
        public string UserRebateId { get; set; }
        public RebateEventSyncStatus SyncStatus { get; set; } = RebateEventSyncStatus.Success;
        public DateTime EventTimestamp { get; set; }
        public RebateEventType RebateEventType { get; set; }
        public UserRebate UserRebate { get; set; }

    }
}