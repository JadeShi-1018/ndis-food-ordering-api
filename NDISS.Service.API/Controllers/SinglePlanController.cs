using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.Common;
using NDISS.Service.API.DTOs.SignlePlan;
using NDISS.Service.API.DTOs.SinglePlan;
using NDISS.Service.API.Services;

namespace NDISS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SinglePlanController : ControllerBase
    {
        private readonly ISinglePlanService _singlePlanService;

        public SinglePlanController(ISinglePlanService singlePlanService)
        {
            _singlePlanService = singlePlanService;
        }

        // POST: api/SinglePlan
        [HttpPost]
        public async Task<IActionResult> CreateSinglePlan([FromBody] SinglePlanCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var created = await _singlePlanService.AddSinglePlanAsync(dto);

            return Ok(ApiResponse<SinglePlanResponseDto>.Success(
                created,
                "SinglePlan created successfully."
            ));
        }
    }
}
