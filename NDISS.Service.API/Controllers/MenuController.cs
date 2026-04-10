using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.Common;
using NDISS.Service.API.DTOs.Menu;
using NDISS.Service.API.Services;

namespace NDISS.Service.API.Controllers
{
    [Route("api/ProviderService/{providerServiceId}/Categories/{categoryId}/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
    private readonly IMenuService _service;

    public MenuController(IMenuService service)
    {
      _service = service;
    }

    // GET: api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menus
    [HttpGet]
    public async Task<IActionResult> GetAllMenus(string providerServiceId, string categoryId)
    {
      var menus = await _service.GetAllMenusAsync(providerServiceId, categoryId);

      return Ok(ApiResponse<IEnumerable<MenuResponseDto>>.Success(
          menus,
          "Retrieved all menus for the category successfully."
      ));
    }

    // GET: api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menus/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenuById(string providerServiceId, string categoryId, string id)
    {
      var menu = await _service.GetMenuByIdAsync(providerServiceId, categoryId, id);

      if (menu == null)
        return NotFound(ApiResponse<object>.Fail("Menu not found.", "404"));

      return Ok(ApiResponse<MenuResponseDto>.Success(
          menu,
          "Menu retrieved successfully."
      ));
    }

   

    // POST: api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menus
    [HttpPost]
    public async Task<IActionResult> CreateMenu(
        string providerServiceId,
        string categoryId,
        [FromBody] MenuCreateDto dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

      var created = await _service.AddMenuAsync(providerServiceId, categoryId, dto);

      return Ok(ApiResponse<MenuResponseDto>.Success(
          created,
          "Menu created successfully."
      ));
    }

    // PUT: api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menus/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenu(
        string providerServiceId,
        string categoryId,
        string id,
        [FromBody] MenuUpdateDto dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

      var updated = await _service.UpdateMenuAsync(providerServiceId, categoryId, id, dto);

      return Ok(ApiResponse<MenuResponseDto>.Success(
          updated,
          "Menu updated successfully."
      ));
    }

    // DELETE: api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menus/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenu(string providerServiceId, string categoryId, string id)
    {
      await _service.DeleteMenuAsync(providerServiceId, categoryId, id);

      return Ok(ApiResponse<object?>.Success(
          null,
          "Menu deleted successfully."
      ));
    }

    [HttpGet("{id}/order-info")]
    // GET: api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menus/{id}/order-info
    public async Task<IActionResult> GetMenuOrderInfo(string providerServiceId, string categoryId, string id)
    {
      var menu = await _service.GetMenuOrderInfoAsync(providerServiceId, categoryId, id);

      if (menu == null)
        return NotFound(ApiResponse<object>.Fail("Menu not found.", "404"));

      return Ok(ApiResponse<MenuOrderInfoResponseDto>.Success(
          menu,
          "Menu retrieved successfully."
      ));
    }

  }


}
