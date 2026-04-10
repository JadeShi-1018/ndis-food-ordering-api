using Microsoft.AspNetCore.Mvc;
using NDIS.Service.API.Services;
using NDIS.Service.API.Common;
using NDIS.Service.API.DTOs.ProviderService;

namespace NDIS.Service.API.Controllers
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

    // GET: api/ProviderService
    [HttpGet]
    public async Task<IActionResult> GetAllProviderServices()
    {
      var providerServices = await _service.GetAllProviderServicesAsync();

      return Ok(ApiResponse<IEnumerable<ProviderServiceListDto>>.Success(
          providerServices,
          "ProviderServices retrieved successfully."
      ));

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProviderServiceById(string id)
    {
      try
      {
        var providerService = await _service.GetProviderServiceByIdAsync(id);

        if (providerService == null)
        {
          return NotFound(ApiResponse<object>.Fail($"ProviderService with id '{id}' not found."));
        }

        return Ok(ApiResponse<ProviderServiceDetailDto>.Success(providerService));
      }
      catch (Exception ex)
      {
        //_logger.LogError(ex, "Error occurred while retrieving ProviderService with id: {ProviderServiceId}", id);
        return StatusCode(500, ApiResponse<object>.Fail("An error occurred while processing your request."));
      }
    }

  }
}
