using System.ComponentModel.DataAnnotations;

namespace NDIS.Service.API.DTOs.Category
{
    public class CategoryBaseDto
    {
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
