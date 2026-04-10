namespace NDIS.Payment.API.Dtos
{
  public class PayOrderRequestDto
  {
    public string OrderId { get; set; } = null!;
    public string PaymentMethod { get; set; } = "StripeTestCard";
  }
}
