namespace NDISS.Service.API.DTOs
{
    public class ProviderServiceLocationDto
    {
        public string ProviderServiceLocationId { get; set; }
        public string ProviderServiceLocationAddress { get; set; }
        public string ProviderServiceLocationCity { get; set; }
        public string ProviderServiceLocationState { get; set; }
        public int ProviderServiceLocationPostcode { get; set; }
        public float ProviderServiceLocationLat { get; set; }
        public float ProviderServiceLocationLong { get; set; }
        public string ProviderServiceId { get; set; }
        public string? ProviderServiceName { get; set; }
    }

}
