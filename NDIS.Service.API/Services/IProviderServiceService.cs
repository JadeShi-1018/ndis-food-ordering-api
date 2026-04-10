using NDIS.Service.API.DTOs.ProviderService;

namespace NDIS.Service.API.Services
{
    public interface IProviderServiceService
    {
        Task<ProviderServiceResponseDto> AddProviderServiceAsync(ProviderServiceCreateDto dto);

        Task<IEnumerable<ProviderServiceListDto>> GetAllProviderServicesAsync();
    Task<ProviderServiceDetailDto?> GetProviderServiceByIdAsync(string providerServiceId);
  }
}
