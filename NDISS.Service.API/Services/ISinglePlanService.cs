using NDISS.Service.API.DTOs.SignlePlan;
using NDISS.Service.API.DTOs.SinglePlan;

namespace NDISS.Service.API.Services
{
    public interface ISinglePlanService
    {
        Task<SinglePlanResponseDto> AddSinglePlanAsync(SinglePlanCreateDto dto);
    }
}