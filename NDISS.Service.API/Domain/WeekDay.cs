using System.ComponentModel.DataAnnotations;

namespace NDISS.Service.API.Domain
{
    public class WeekDay
    {
        [Key]
        public string WeekDayId { get; set; }

        [Required]
        public string WeekDayName { get; set; }

        public ICollection<SinglePlan> SinglePlans { get; set; } = new List<SinglePlan>();
    }
}
