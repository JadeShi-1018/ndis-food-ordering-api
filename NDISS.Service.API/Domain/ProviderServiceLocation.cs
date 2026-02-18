using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class ProviderServiceLocation
    {
        [Key]
        public string ProviderServiceLocationId { get; set; }

        [Required]
        public string ProviderServiceLocationAddress { get; set; }

        [Required]
        public string ProviderServiceLocationCity { get; set; }

        [Required]
        public string ProviderServiceLocationState { get; set; }

        public int ProviderServiceLocationPostcode { get; set; }

        public float ProviderServiceLocationLat { get; set; }

        public float ProviderServiceLocationLong { get; set; }

        // Foreign Key
        public string ProviderServiceId { get; set; }

        [ForeignKey("ProviderServiceId")]
        public ProviderService? ProviderService { get; set; }
    }
}
