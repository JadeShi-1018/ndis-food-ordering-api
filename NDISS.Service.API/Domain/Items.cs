using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class Item
    {
        [Key]
        public string ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string? ItemDescription { get; set; }

        // Foreign Key
        public string ProviderServiceId { get; set; }

        [ForeignKey("ProviderServiceId")]
        public ProviderService? ProviderService { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
