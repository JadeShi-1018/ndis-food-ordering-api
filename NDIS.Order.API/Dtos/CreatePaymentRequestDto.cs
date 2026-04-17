namespace NDIS.Order.API.Dtos
{
  public class CreatePaymentRequestDto
  {
    public string OrderId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string IdempotencyKey { get; set; } = null!;
  }
}
