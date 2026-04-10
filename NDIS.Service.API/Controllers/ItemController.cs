using Microsoft.AspNetCore.Mvc;
using NDIS.Service.API.Common;
using NDISServiceAPI.DTO.Item;
using NDISServiceAPI.Services.ItemService;

namespace NDISServiceAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ItemController : ControllerBase
  {
    private readonly IItemService _itemService;
    private readonly ILogger<ItemController> _logger;

    public ItemController(IItemService itemService, ILogger<ItemController> logger)
    {
      _itemService = itemService;
      _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] CreateItemRequestDto request)
    {
      if (request == null)
      {
        return BadRequest(ApiResponse<object>.Fail("Item data is null.", "400"));
      }

      var result = await _itemService.AddItem(request);

      if (!result)
      {
        return StatusCode(500, ApiResponse<object>.Fail("Failed to add item.", "500"));
      }

      return Ok(ApiResponse<object>.Success(result, "Item added successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
      var items = await _itemService.GetAllItems();
      return Ok(ApiResponse<IEnumerable<ItemResponseDto>>.Success(items, "Items retrieved successfully."));
    }

    [HttpGet("{itemId}")]
    public async Task<IActionResult> GetById([FromRoute] string itemId)
    {
      if (string.IsNullOrWhiteSpace(itemId))
      {
        return BadRequest(ApiResponse<object>.Fail("ItemId cannot be null or empty.", "400"));
      }

      var item = await _itemService.GetById(itemId);

      if (item == null)
      {
        return NotFound(ApiResponse<object>.Fail($"Item with ID {itemId} not found.", "404"));
      }

      return Ok(ApiResponse<ItemResponseDto>.Success(item, "Item retrieved successfully."));
    }

    [HttpPut("{itemId}")]
    public async Task<IActionResult> Update([FromRoute] string itemId, [FromBody] CreateItemRequestDto request)
    {
      if (string.IsNullOrWhiteSpace(itemId) || request == null)
      {
        return BadRequest(ApiResponse<object>.Fail("ItemId or request data cannot be null or empty.", "400"));
      }

      var result = await _itemService.Update(itemId, request);

      if (!result)
      {
        return NotFound(ApiResponse<object>.Fail($"Item with ID {itemId} not found.", "404"));
      }

      return Ok(ApiResponse<object>.Success(result, "Item updated successfully."));
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> Delete([FromRoute] string itemId)
    {
      if (string.IsNullOrWhiteSpace(itemId))
      {
        return BadRequest(ApiResponse<object>.Fail("ItemId cannot be null or empty.", "400"));
      }

      var result = await _itemService.Delete(itemId);

      if (!result)
      {
        return NotFound(ApiResponse<object>.Fail($"Item with ID {itemId} not found.", "404"));
      }

      return Ok(ApiResponse<object>.Success(result, "Item deleted successfully."));
    }
  }
}