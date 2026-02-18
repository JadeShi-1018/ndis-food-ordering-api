using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.Services;
using NDISS.Service.API.Common;
using NDISS.Service.API.DTOs.ServiceType;

namespace NDISS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _service;

        public ServiceTypeController(IServiceTypeService service)
        {
            _service = service;
        }

        // GET: api/ServiceType
        [HttpGet]
        public async Task<IActionResult> GetAllServiceTypes()
        {
            var serviceTypes = await _service.GetAllServiceTypesAsync();
            return Ok(ApiResponse<IEnumerable<ServiceTypeResponseDto>>.Success(serviceTypes));
        }

        // GET: api/ServiceType/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceTypeById(string id)
        {
            var serviceType = await _service.GetServiceTypeByIdAsync(id);
            return Ok(ApiResponse<ServiceTypeResponseDto>.Success(serviceType!));
        }

        // POST: api/ServiceType
        [HttpPost]
        public async Task<IActionResult> CreateServiceType([FromBody] ServiceTypeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var created = await _service.AddServiceTypeAsync(dto);
            return Ok(ApiResponse<ServiceTypeResponseDto>.Success(created, "ServiceType created successfully."));
        }

        // PUT: api/ServiceType/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceType(string id, [FromBody] ServiceTypeUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var updated = await _service.UpdateServiceTypeAsync(id, dto);
            return Ok(ApiResponse<ServiceTypeResponseDto>.Success(updated, "ServiceType updated successfully."));
        }

        // DELETE: api/ServiceType/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceType(string id)
        {
            await _service.DeleteServiceTypeAsync(id);
            return Ok(ApiResponse<object?>.Success(null, "ServiceType deleted successfully."));
        }
    }
}
