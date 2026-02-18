using System.ComponentModel.DataAnnotations;

namespace NDISS.Service.API.DTOs.Category
{
    public class CategoryBaseDto
    {
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
