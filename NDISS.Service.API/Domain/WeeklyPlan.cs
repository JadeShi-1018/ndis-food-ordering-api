using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class WeeklyPlan
    {
        [Key]
        public string WeeklyPlanId { get; set; }

        [Required]
        public string PlanName { get; set; }

        public string? PlanDescription { get; set; }

        public float PlanPrice { get; set; }

        // Foreign Key
        public string CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<SinglePlan> SinglePlans { get; set; } = new List<SinglePlan>();
    }
}
