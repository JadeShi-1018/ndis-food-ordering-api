using NDISS.Service.API.DTOs.Category;

namespace NDISS.Service.API.DTOs.WeeklyPlan
{
    public class WeeklyPlanResponseDto
    {
        public string WeeklyPlanId { get; set; }

        public string PlanName { get; set; }

        public string? PlanDescription { get; set; }

        public decimal PlanPrice { get; set; }

        public string CategoryId { get; set; }

        public CategoryResponseDto? Category { get; set; }

        //public List<SinglePlanResponseDto> SinglePlans { get; set; } = new();
    }
}
