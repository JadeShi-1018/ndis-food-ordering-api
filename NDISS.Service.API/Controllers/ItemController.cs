using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.Common;
using NDISS.Service.API.DTOs.Category;
using NDISServiceAPI.DTO.Item;
using NDISServiceAPI.Services.ItemService;

namespace NDISServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<IItemService> _logger;

        public ItemController(IItemService itemService, ILogger<IItemService> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] CreateItemRequestDto request)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse<object>.Fail("Item data is null.", "400"));
            }
            var result = await _itemService.AddItem(request);

            if (!result)
            {
                return BadRequest(ApiResponse<object>.Fail("Failed to add item.", "500"));
            }

            return Ok(ApiResponse<object>.Success(
                result,
                "Item added successfully."
            ));
 
        }

        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItems();
            return Ok(ApiResponse<IEnumerable<ItemResponseDto>>.Success(items, "Items retrieved successfully."));
            
        }

        [HttpGet("GetById/{itemId}")]
        public async Task<IActionResult> GetById([FromRoute] string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return BadRequest(ApiResponse<object>.Fail("ItemId cannot be null or empty.", "400"));
            }


            var item = await _itemService.GetById(itemId);
            if (item == null)
            {
                return BadRequest(ApiResponse<object>.Fail($"Item with ID {itemId} not found.", "404"));
            }

            return Ok(ApiResponse<ItemResponseDto>.Success(item, "Item retrieved successfully."));
           
        }

        [HttpPut("Update/{itemId}")]
        public async Task<IActionResult> Update([FromRoute] string itemId, [FromBody] CreateItemRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(itemId) || request == null)
            {
                return Ok(ApiResponse<object>.Fail("ItemId or request data cannot be null or empty.", "400"));
            }

                var result = await _itemService.Update(itemId, request);
                if (result)
                {
                    return Ok(ApiResponse<object>.Success(
                     result,
                     "Item updated successfully."
                    ));
            }
                else
                {
                    return BadRequest(ApiResponse<Object>.Fail($"Item with ID {itemId} not found.", "404"));
                }
       
        }

        [HttpDelete("Delete/{itemId}")]
        public async Task<IActionResult> Delete([FromRoute] string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return BadRequest(ApiResponse<object>.Fail("ItemId cannot be null or empty.", "400"));
            }
            var result = await _itemService.Delete(itemId);
            if (result)
            {
                return Ok(ApiResponse<Object>.Success(result, "Item deleted successfully."));
            }
            else
            {
                return BadRequest(ApiResponse<Object>.Fail($"Item with ID {itemId} not found.", "404"));
            }
          
        }
    }
}
