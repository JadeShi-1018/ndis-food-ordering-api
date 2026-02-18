using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.DTOs;
using NDISS.Service.API.Services;
using NDISS.Service.API.Common;

namespace NDISS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderServiceLocationController: ControllerBase
    {
        private readonly IProviderServiceLocationService _service;

        public ProviderServiceLocationController(IProviderServiceLocationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProviderServiceLocationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input.", "400"));

            var result = await _service.AddAsync(dto);
            return result.Succeed ? Ok(result) : StatusCode(500, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.Succeed ? Ok(result) : StatusCode(500, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Succeed ? Ok(result) : StatusCode(500, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProviderServiceLocationUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input.", "400"));

            var result = await _service.UpdateAsync(id, dto);
            return result.Succeed ? Ok(result) : StatusCode(500, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Succeed ? Ok(result) : StatusCode(500, result);
        }
    }
}
