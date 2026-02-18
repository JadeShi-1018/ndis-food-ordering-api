using System.ComponentModel.DataAnnotations;

namespace NDISS.Service.API.Domain
{
    public class ServiceType
    {
        [Key]
        public string ServiceTypeId { get; set; }

        [Required]
        public string ServiceTypeName { get; set; }

        public string? ServiceDescription { get; set; }

        public ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();
    }
}
