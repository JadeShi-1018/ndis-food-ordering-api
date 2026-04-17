namespace NDIS.Payment.API.Dtos
{
  public class PayPaymentResponseDto
  {
    public string PaymentId { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public string Message { get; set; } = null!;
  }
}
