using AutoMapper;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs;
using NDISS.Service.API.Repositories;
using NDISS.Service.API.Common;

namespace NDISS.Service.API.Services
{
    public class ProviderServiceLocationService:IProviderServiceLocationService
    {
        private readonly IGenericRepository<ProviderServiceLocation> _repository;
        private readonly IMapper _mapper;

        public ProviderServiceLocationService(
            IGenericRepository<ProviderServiceLocation> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<object?>> AddAsync(ProviderServiceLocationCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<ProviderServiceLocation>(dto);
                entity.ProviderServiceLocationId = Guid.NewGuid().ToString();
                await _repository.AddAsync(entity);
                await _repository.SaveAsync();
                return ApiResponse<object?>.Success(null, "Created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object?>.Fail($"Exception: {ex.Message}", "500");
            }
        }

        public async Task<ApiResponse<List<ProviderServiceLocationDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var result = _mapper.Map<List<ProviderServiceLocationDto>>(entities);
            return ApiResponse<List<ProviderServiceLocationDto>>.Success(result);
        }

        public async Task<ApiResponse<ProviderServiceLocationDto?>> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return ApiResponse<ProviderServiceLocationDto?>.Fail("Not found.", "404");

            var dto = _mapper.Map<ProviderServiceLocationDto>(entity);
            return ApiResponse<ProviderServiceLocationDto?>.Success(dto);
        }

        public async Task<ApiResponse<object?>> UpdateAsync(string id, ProviderServiceLocationUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return ApiResponse<object?>.Fail("Not found.", "404");

            _mapper.Map(dto, entity);
            await _repository.Update(entity);
            await _repository.SaveAsync();

            return ApiResponse<object?>.Success(null, "Updated successfully.");
        }

        public async Task<ApiResponse<object?>> DeleteAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return ApiResponse<object?>.Fail("Not found.", "404");

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return ApiResponse<object?>.Success(null, "Deleted successfully.");
        }

    }
}
