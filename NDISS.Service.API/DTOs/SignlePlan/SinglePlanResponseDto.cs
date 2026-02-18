using NDISS.Service.API.DTOs.WeeklyPlan;

namespace NDISS.Service.API.DTOs.SignlePlan
{
    public class SinglePlanResponseDto
    {
        public string SinglePlanId { get; set; }
        public string MenuId { get; set; }
        public string WeeklyPlanId { get; set; }
        public string WeekDayId { get; set; }

        //public MenuResponseDto? Menu { get; set; }
        public WeeklyPlanResponseDto? WeeklyPlan { get; set; }

        //public WeekDayResponseDto? WeekDay { get; set; }
    }
}
