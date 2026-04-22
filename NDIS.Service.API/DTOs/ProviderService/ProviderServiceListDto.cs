namespace NDIS.Service.API.DTOs.ProviderService
{
  public class ProviderServiceListDto
  {
    public string ProviderServiceId { get; set; }

    public string ProviderServiceName { get; set; }

    public string ProviderId { get; set; }

    public string ServiceTypeId { get; set; }

    public string? ServiceTypeName { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public int Postcode { get; set; }

    public float Lat { get; set; }

    public float Long { get; set; }

    public int CategoryCount { get; set; }

    public int ItemCount { get; set; }
  }

}
