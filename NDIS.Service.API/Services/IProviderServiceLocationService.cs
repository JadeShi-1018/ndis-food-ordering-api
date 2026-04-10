using NDIS.Service.API.DTOs;
using NDIS.Service.API.Common;

namespace NDIS.Service.API.Services
{
    public interface IProviderServiceLocationService
    {
        Task<ApiResponse<object?>> AddAsync(ProviderServiceLocationCreateDto dto);
        Task<ApiResponse<List<ProviderServiceLocationDto>>> GetAllAsync();
        Task<ApiResponse<ProviderServiceLocationDto?>> GetByIdAsync(string id);
        Task<ApiResponse<object?>> UpdateAsync(string id, ProviderServiceLocationUpdateDto dto);
        Task<ApiResponse<object?>> DeleteAsync(string id);
    }
}
