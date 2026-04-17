using NDIS.Order.API.Dtos;

namespace NDIS.Order.API.ServiceClient
{
  public interface IPaymentServiceClient
  {
    Task<CreatePaymentResponseDto> CreatePaymentAsync(CreatePaymentRequestDto request);
  }
}
