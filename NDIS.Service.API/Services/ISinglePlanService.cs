using NDIS.Service.API.DTOs.SignlePlan;
using NDIS.Service.API.DTOs.SinglePlan;

namespace NDIS.Service.API.Services
{
    public interface ISinglePlanService
    {
        Task<SinglePlanResponseDto> AddSinglePlanAsync(SinglePlanCreateDto dto);
    }
}