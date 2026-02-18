using NDISS.Service.API.DTOs.WeeklyPlan;

namespace NDISS.Service.API.Services
{
    public interface IWeeklyPlanService
    {
        Task<WeeklyPlanResponseDto> AddWeeklyPlanAsync(WeeklyPlanCreateDto dto);

        Task<IEnumerable<WeeklyPlanResponseDto>> GetAllWeeklyPlansAsync();

        Task<WeeklyPlanResponseDto?> GetWeeklyPlanByIdAsync(string id);


    }
}
