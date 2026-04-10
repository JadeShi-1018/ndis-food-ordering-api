using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDIS.Service.API.Domain
{
    public class ProviderService
    {
        [Key]
        public string ProviderServiceId { get; set; }

    [Required]
    public string ProviderServiceName { get; set; }

    [Required]
        public string ProviderId { get; set; }

    // Foreign Key
    [Required]
        public string ServiceTypeId { get; set; }

        [ForeignKey("ServiceTypeId")]
        public ServiceType? ServiceType { get; set; }

        public string? PhoneNumber { get; set; }
        public string? OpeningHours { get; set; }

    // Location (simplify the relationship between ProviderService and Location)
    [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        public int Postcode { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
