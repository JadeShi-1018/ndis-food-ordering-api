using NDISS.Service.API.DTOs.Category;
using NDISS.Service.API.DTOs.ServiceType;

namespace NDISS.Service.API.DTOs.ProviderService
{
    public class ProviderServiceResponseDto
    {
        public string ProviderServiceId { get; set; }

        public string ProviderId { get; set; }

        public string ServiceTypeId { get; set; }

        public ServiceTypeResponseDto? ServiceType { get; set; }

        public List<CategoryResponseDto> Categories { get; set; } = new();

        //public List<ProviderServiceLocationDto> ProviderServiceLocations { get; set; } = new();

        //public List<ItemDto> Items { get; set; } = new();
    }
}
