using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NDISS.UserRebate.API.Enums;

namespace NDISS.UserRebate.API.Domain
{
    public class RebateRetrySchedule
    {
        [Key]
        public string ScheduleId { get; set; }

        [ForeignKey("RebateEvent")]
        public string RebateEventId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public RebateRetryStatus Status { get; set; } = RebateRetryStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int RetryCount { get; set; }
        public string RetryReason { get; set; }
        public RebateEvent RebateEvent { get; set; }

    }
}