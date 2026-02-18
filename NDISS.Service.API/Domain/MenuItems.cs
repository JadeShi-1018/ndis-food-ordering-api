using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDISS.Service.API.Domain
{
    public class MenuItem
    {
        [Key]
        public string MenuItemId { get; set; }

        // Foreign Key
        public string MenuId { get; set; }

        public string ItemId { get; set; }

        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
    }
}
