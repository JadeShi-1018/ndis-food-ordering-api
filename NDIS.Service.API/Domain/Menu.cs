using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDIS.Service.API.Domain
{
    public class Menu
    {
        [Key]
        public string MenuId { get; set; }
        [Required]
        public string MenuName { get; set; }
        public string? Description { get; set; }

    // // Foreign Key
    // public string PeriodId { get; set; }
    [Required]
        public MenuPeriod Period { get; set; }

    [Required]
        public string CategoryId { get; set; }

    // [ForeignKey("PeriodId")]
    // public Period? Period { get; set; }
      [Required]
    public decimal Price { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
