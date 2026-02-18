using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs.Category;
using NDISS.Service.API.Repositories;
using NDIS.Shared.Common.Extensions;


namespace NDISS.Service.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(string providerServiceId)
        {
            var categories = await _categoryRepository.GetByProviderServiceIdAsync(providerServiceId);
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(string providerServiceId, string categoryId)
        {
            var category = await _categoryRepository.GetByIdAndProviderAsync(categoryId, providerServiceId);
            if (category == null)
            {
                _logger.LogWarning($"Category not found: {categoryId} for provider: {providerServiceId}");
                return null;
            }

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> AddCategoryAsync(string providerServiceId, CategoryCreateDto dto)
        {
            var provider = await _categoryRepository
                .GetDbSet<ProviderService>()
                .FirstOrDefaultAsync(p => p.ProviderServiceId == providerServiceId);

            if (provider == null)
                throw new ResourceNotFoundException($"ProviderService {providerServiceId} not found.");

            var category = _mapper.Map<Category>(dto);
            category.CategoryId = Guid.NewGuid().ToString();
            category.ProviderServiceId = providerServiceId;

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(string providerServiceId, string categoryId, CategoryUpdateDto dto)
        {
            var category = await _categoryRepository.GetByIdAndProviderAsync(categoryId, providerServiceId);
            if (category == null)
                throw new ResourceNotFoundException("Category not found.");

            category.CategoryName = dto.CategoryName;
            category.CategoryDescription = dto.CategoryDescription;

            await _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task DeleteCategoryAsync(string providerServiceId, string categoryId)
        {
            var category = await _categoryRepository.GetByIdAndProviderAsync(categoryId, providerServiceId);
            if (category == null)
                throw new ResourceNotFoundException("Category not found.");

            _categoryRepository.GetDbSet<Category>().Remove(category);
            await _categoryRepository.SaveAsync();
        }
    }
}
