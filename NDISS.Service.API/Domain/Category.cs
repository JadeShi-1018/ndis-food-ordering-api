using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        // Foreign Key
        public string ProviderServiceId { get; set; }

        [ForeignKey("ProviderServiceId")]
        public ProviderService? ProviderService { get; set; }

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();

        public ICollection<WeeklyPlan> WeeklyPlans { get; set; } = new List<WeeklyPlan>();
    }
}
