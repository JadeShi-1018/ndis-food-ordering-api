using NDISS.Service.API.DTOs.ProviderService;

namespace NDISS.Service.API.Services
{
    public interface IProviderServiceService
    {
        Task<ProviderServiceResponseDto> AddProviderServiceAsync(ProviderServiceCreateDto dto);

        Task<IEnumerable<ProviderServiceListDto>> GetAllProviderServicesAsync();
    Task<ProviderServiceDetailDto?> GetProviderServiceByIdAsync(string providerServiceId);
  }
}
