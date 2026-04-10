namespace NDIS.Order.API.Dtos
{
  public class MenuDetailDto
  {
    public string ProviderId { get; set; } = null!;
    public string ProviderServiceId { get; set; } = null!;
    public string ProviderServiceName { get; set; } = null!;
    public string? ProviderPhoneNumber { get; set; }

    public string CategoryId { get; set; } = null!;
    public string CategoryName { get; set; } = null!;

    public string MenuId { get; set; } = null!;
    public string MenuName { get; set; } = null!;
    public string? MenuDescription { get; set; }

    public string PeriodName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
  }
}
