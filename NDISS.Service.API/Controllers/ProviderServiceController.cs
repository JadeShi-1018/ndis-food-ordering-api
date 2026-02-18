using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.Services;
using NDISS.Service.API.Common;
using NDISS.Service.API.DTOs.ProviderService;

namespace NDISS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderServiceController : ControllerBase
    {
        private readonly IProviderServiceService _service;

        public ProviderServiceController(IProviderServiceService service)
        {
            _service = service;
        }

        // POST: api/ProviderService
        [HttpPost]
        public async Task<IActionResult> CreateProviderService([FromBody] ProviderServiceCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var created = await _service.AddProviderServiceAsync(dto);
            return Ok(ApiResponse<ProviderServiceResponseDto>.Success(
                created,
                "ProviderService created successfully."
            ));
        }
    }
}
