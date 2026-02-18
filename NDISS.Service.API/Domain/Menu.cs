using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class Menu
    {
        [Key]
        public string MenuId { get; set; }

        // Foreign Key
        public string PeriodId { get; set; }

        public string CategoryId { get; set; }

        [ForeignKey("PeriodId")]
        public Period? Period { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
