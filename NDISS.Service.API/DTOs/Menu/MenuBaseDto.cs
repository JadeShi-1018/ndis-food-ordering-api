using System.ComponentModel.DataAnnotations;

namespace NDISS.Service.API.DTOs.Menu
{
  public class MenuBaseDto
  {
    public string MenuName { get; set; }

    public string PeriodName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
  }
}
