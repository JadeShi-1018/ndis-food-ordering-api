using NDISS.Service.API.DTOs.Category;

namespace NDISS.Service.API.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(string providerServiceId);

        Task<CategoryResponseDto?> GetCategoryByIdAsync(string providerServiceId, string categoryId);

        Task<CategoryResponseDto> AddCategoryAsync(string providerServiceId, CategoryCreateDto dto);

        Task<CategoryResponseDto> UpdateCategoryAsync(string providerServiceId, string categoryId, CategoryUpdateDto dto);

        Task DeleteCategoryAsync(string providerServiceId, string categoryId);
    }

}
