using Microsoft.AspNetCore.Mvc;
using NDISS.Service.API.Services;
using NDISS.Service.API.Common;
using NDISS.Service.API.DTOs.Category;

namespace NDISS.Service.API.Controllers
{
    [ApiController]
    [Route("api/ProviderService/{providerServiceId}/Categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/ProviderService/{providerServiceId}/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(string providerServiceId)
        {
            var categories = await _service.GetAllCategoriesAsync(providerServiceId);
            return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.Success(
                categories,
                "Retrieved all categories for the provider successfully."
            ));
        }

        // GET: api/ProviderService/{providerServiceId}/Categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string providerServiceId, string id)
        {
            var category = await _service.GetCategoryByIdAsync(providerServiceId, id);
            if (category == null)
                return NotFound(ApiResponse<object>.Fail("Category not found.", "404"));

            return Ok(ApiResponse<CategoryResponseDto>.Success(
                category,
                "Category retrieved successfully."
            ));
        }

        // POST: api/ProviderService/{providerServiceId}/Categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory(string providerServiceId, [FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var created = await _service.AddCategoryAsync(providerServiceId, dto);
            return Ok(ApiResponse<CategoryResponseDto>.Success(
                created,
                "Category created successfully."
            ));
        }

        // PUT: api/ProviderService/{providerServiceId}/Categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string providerServiceId, string id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid input parameters.", "400"));

            var updated = await _service.UpdateCategoryAsync(providerServiceId, id, dto);
            return Ok(ApiResponse<CategoryResponseDto>.Success(
                updated,
                "Category updated successfully."
            ));
        }

        // DELETE: api/ProviderService/{providerServiceId}/Categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string providerServiceId, string id)
        {
            await _service.DeleteCategoryAsync(providerServiceId, id);
            return Ok(ApiResponse<object?>.Success(
                null,
                "Category deleted successfully."
            ));
        }
    }
}
