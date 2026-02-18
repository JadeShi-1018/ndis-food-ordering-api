using NDISS.Service.API.DTOs.ServiceType;

namespace NDISS.Service.API.Services
{
    public interface IServiceTypeService
    {
        Task<IEnumerable<ServiceTypeResponseDto>> GetAllServiceTypesAsync();

        Task<ServiceTypeResponseDto?> GetServiceTypeByIdAsync(string id);

        Task<ServiceTypeResponseDto> AddServiceTypeAsync(ServiceTypeCreateDto dto);

        Task<ServiceTypeResponseDto> UpdateServiceTypeAsync(string id, ServiceTypeUpdateDto dto);

        Task DeleteServiceTypeAsync(string id);
    }
}
