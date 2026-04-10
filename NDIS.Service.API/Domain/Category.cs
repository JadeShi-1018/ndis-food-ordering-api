using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDIS.Service.API.Domain
{
    public class Category
    {
        [Key]
        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

    // Foreign Key
    [Required]
        public string ProviderServiceId { get; set; }

        [ForeignKey("ProviderServiceId")]
        public ProviderService? ProviderService { get; set; }

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();

        public ICollection<WeeklyPlan> WeeklyPlans { get; set; } = new List<WeeklyPlan>();
    }
}
