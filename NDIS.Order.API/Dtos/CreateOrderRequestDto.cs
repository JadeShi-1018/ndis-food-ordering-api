namespace NDIS.Order.API.Dtos
{
  public class CreateOrderRequestDto
  {


    public string ProviderServiceId { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public string MenuId { get; set; } = null!;
    public string IdempotencyKey { get; set; } = null!;


    //public int Quantity { get; set; } 


    public string DeliveryAddress { get; set; } = null!;  
    //public string CustomerContactNumber { get; set; } = null!; 
    //public string ProviderContactNumber { get; set; } = null!; //User -- provider

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
  }
}
