using NDIS.Payment.API.Dtos;

namespace NDIS.Payment.API.ServiceClient
{
  public interface IOrderServiceClient
  {
    Task UpdateOrderStatusAsync(string orderId, UpdateOrderStatusRequestDto request);
  }
}
