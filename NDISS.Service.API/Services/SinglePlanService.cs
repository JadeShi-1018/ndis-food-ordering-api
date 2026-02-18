using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDISS.Service.API.Common;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs.SignlePlan;
using NDISS.Service.API.DTOs.SinglePlan;
using NDISS.Service.API.Repositories;
using NDIS.Shared.Common.Extensions;


namespace NDISS.Service.API.Services
{
    public class SinglePlanService : ISinglePlanService
    {
        private readonly IGenericRepository<SinglePlan> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SinglePlanService> _logger;

        public SinglePlanService(
            IGenericRepository<SinglePlan> repository,
            IMapper mapper,
            ILogger<SinglePlanService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SinglePlanResponseDto> AddSinglePlanAsync(SinglePlanCreateDto dto)
        {
            try
            {
                // Load related entities directly
                var menu = await _repository.GetDbSet<Menu>()
                    .FirstOrDefaultAsync(m => m.MenuId == dto.MenuId);
                if (menu == null)
                    throw new ResourceNotFoundException($"Menu {dto.MenuId} not found.");

                var weeklyPlan = await _repository.GetDbSet<WeeklyPlan>()
                    .FirstOrDefaultAsync(w => w.WeeklyPlanId == dto.WeeklyPlanId);
                if (weeklyPlan == null)
                    throw new ResourceNotFoundException($"WeeklyPlan {dto.WeeklyPlanId} not found.");

                var weekDay = await _repository.GetDbSet<WeekDay>()
                    .FirstOrDefaultAsync(wd => wd.WeekDayId == dto.WeekDayId);
                if (weekDay == null)
                    throw new ResourceNotFoundException($"WeekDay {dto.WeekDayId} not found.");

                // Create SinglePlan entity
                var singlePlan = _mapper.Map<SinglePlan>(dto);
                singlePlan.SinglePlanId = GenerateId();
                singlePlan.Menu = menu;
                singlePlan.WeeklyPlan = weeklyPlan;
                singlePlan.WeekDay = weekDay;

                await _repository.AddAsync(singlePlan);
                await _repository.SaveAsync();

                // Return response
                return _mapper.Map<SinglePlanResponseDto>(singlePlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add SinglePlan.");
                throw;
            }
        }
        private string GenerateId() => Guid.NewGuid().ToString();
    }
}
