namespace NDIS.Payment.API.Dtos
{
  public class PaymentResponseDto
  {
    public string PaymentId { get; set; } = null!;
    public string OrderId { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public decimal PaymentPrice { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
