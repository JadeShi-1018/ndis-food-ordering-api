namespace NDISS.Service.API.DTOs.WeeklyPlan
{
    public class WeeklyPlanBaseDto
    {
        public string PlanName { get; set; }

        public string? PlanDescription { get; set; }

        public decimal PlanPrice { get; set; }

        public string CategoryId { get; set; }
    }
}