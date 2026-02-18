using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.DTOs.WeeklyPlan;
using NDISS.Service.API.Common;
using NDISS.Service.API.Services;

namespace NDISS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeeklyPlanController : ControllerBase
    {
        private readonly IWeeklyPlanService _weeklyPlanService;

        public WeeklyPlanController(IWeeklyPlanService weeklyPlanService)
        {
            _weeklyPlanService = weeklyPlanService;
        }

        // POST: api/WeeklyPlan
        [HttpPost]
        public async Task<IActionResult> CreateWeeklyPlan([FromBody] WeeklyPlanCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var created = await _weeklyPlanService.AddWeeklyPlanAsync(dto);
            return Ok(ApiResponse<WeeklyPlanResponseDto>.Success(
                created,
                "WeeklyPlan created successfully."
            ));
        }

        // GET: api/WeeklyPlan
        [HttpGet]
        public async Task<IActionResult> GetAllWeeklyPlans()
        {
            var plans = await _weeklyPlanService.GetAllWeeklyPlansAsync();
            return Ok(ApiResponse<IEnumerable<WeeklyPlanResponseDto>>.Success(
                plans,
                "WeeklyPlans retrieved successfully."
            ));
        }

        // GET: api/WeeklyPlan/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeeklyPlanById(string id)
        {
            var plan = await _weeklyPlanService.GetWeeklyPlanByIdAsync(id);
            if (plan == null)
                return NotFound(ApiResponse<object>.Fail($"WeeklyPlan {id} not found.", "404"));

            return Ok(ApiResponse<WeeklyPlanResponseDto>.Success(
                plan,
                "WeeklyPlan retrieved successfully."
            ));
        }
    }
}
