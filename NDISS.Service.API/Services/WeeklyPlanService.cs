using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs.WeeklyPlan;
using NDISS.Service.API.Repositories;
using NDIS.Shared.Common.Extensions;

namespace NDISS.Service.API.Services
{
    public class WeeklyPlanService : IWeeklyPlanService
    {
        private readonly IGenericRepository<WeeklyPlan> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<WeeklyPlanService> _logger;

        public WeeklyPlanService(
            IGenericRepository<WeeklyPlan> repository,
            IMapper mapper,
            ILogger<WeeklyPlanService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WeeklyPlanResponseDto> AddWeeklyPlanAsync(WeeklyPlanCreateDto dto)
        {
            try
            {
                var categoryExists = await _repository.GetDbSet<Category>()
                    .AnyAsync(c => c.CategoryId == dto.CategoryId);

                if (!categoryExists)
                    throw new ResourceNotFoundException($"Category {dto.CategoryId} not found.");

                var weeklyPlan = _mapper.Map<WeeklyPlan>(dto);
                weeklyPlan.WeeklyPlanId = GenerateId();

                await _repository.AddAsync(weeklyPlan);
                await _repository.SaveAsync();

                weeklyPlan.Category = await _repository
                    .GetDbSet<Category>()
                    .FirstOrDefaultAsync(c => c.CategoryId == weeklyPlan.CategoryId);

                return _mapper.Map<WeeklyPlanResponseDto>(weeklyPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new WeeklyPlan.");
                throw;
            }
        }

        public async Task<IEnumerable<WeeklyPlanResponseDto>> GetAllWeeklyPlansAsync()
        {
            try
            {
                var plans = await _repository.GetDbSet<WeeklyPlan>()
                    .Include(wp => wp.Category)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<WeeklyPlanResponseDto>>(plans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all WeeklyPlans.");
                throw;
            }
        }

        public async Task<WeeklyPlanResponseDto?> GetWeeklyPlanByIdAsync(string id)
        {
            try
            {
                var plan = await _repository.GetDbSet<WeeklyPlan>()
                    .Include(wp => wp.Category)
                    .FirstOrDefaultAsync(wp => wp.WeeklyPlanId == id);

                if (plan == null)
                {
                    _logger.LogWarning($"WeeklyPlan not found: {id}");
                    return null;
                }

                return _mapper.Map<WeeklyPlanResponseDto>(plan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching WeeklyPlan with ID: {id}");
                throw;
            }
        }

        private string GenerateId() => Guid.NewGuid().ToString();
    }
}
