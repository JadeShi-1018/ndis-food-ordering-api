using NDIS.Payment.API.Dtos;

namespace NDIS.Payment.API.Services
{
  public interface IPaymentService
  {
    Task<CreatePaymentResponseDto> CreatePaymentAsync(CreatePaymentRequestDto request);
    Task<PayPaymentResponseDto> PayAsync(string paymentId, PayPaymentRequestDto request);
  }
}