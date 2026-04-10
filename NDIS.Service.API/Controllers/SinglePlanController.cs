using Microsoft.AspNetCore.Mvc;
using NDIS.Service.API.Common;
using NDIS.Service.API.DTOs.SignlePlan;
using NDIS.Service.API.DTOs.SinglePlan;
using NDIS.Service.API.Services;

namespace NDIS.Service.API.Controllers
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
