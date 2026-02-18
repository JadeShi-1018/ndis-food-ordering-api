using System.ComponentModel.DataAnnotations;

namespace NDISS.Service.API.Domain
{
    public class Period
    {
        [Key]
        public string PeriodId { get; set; }

        [Required]
        public string PeriodName { get; set; }

        public DateTime DeliveryTime { get; set; }

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
    }
}
