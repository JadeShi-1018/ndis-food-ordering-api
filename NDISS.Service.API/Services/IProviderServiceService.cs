using NDISS.Service.API.DTOs.ProviderService;

namespace NDISS.Service.API.Services
{
    public interface IProviderServiceService
    {
        Task<ProviderServiceResponseDto> AddProviderServiceAsync(ProviderServiceCreateDto dto);
    }
}
