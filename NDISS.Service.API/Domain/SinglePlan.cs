using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class SinglePlan
    {
        [Key]
        public string SinglePlanId { get; set; }

        // Foreign Key
        public string MenuId { get; set; }

        public string WeeklyPlanId { get; set; }

        public string WeekDayId { get; set; }

        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

        [ForeignKey("WeeklyPlanId")]
        public WeeklyPlan? WeeklyPlan { get; set; }

        [ForeignKey("WeekDayId")]
        public WeekDay? WeekDay { get; set; }
    }
}
