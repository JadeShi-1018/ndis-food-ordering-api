namespace NDISS.Service.API.DTOs.ProviderService
{
    public class ProviderServiceCreateDto : ProviderServiceBaseDto
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Postcode { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }
    }
}
