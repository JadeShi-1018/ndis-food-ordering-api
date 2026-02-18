using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class ProviderService
    {
        [Key]
        public string ProviderServiceId { get; set; }

        [Required]
        public string ProviderId { get; set; }

        // Foreign Key
        public string ServiceTypeId { get; set; }

        [ForeignKey("ServiceTypeId")]
        public ServiceType? ServiceType { get; set; }

        public ICollection<ProviderServiceLocation> ProviderServiceLocations { get; set; } = new List<ProviderServiceLocation>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
