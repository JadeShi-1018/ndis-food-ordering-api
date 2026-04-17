namespace NDIS.Payment.API.Dtos
{
  public class CreatePaymentResponseDto
  {
    public string PaymentId { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
  }
}
