using NDIS.Order.API.Dtos;

namespace NDIS.Order.API.ServiceClient
{
  public interface IServiceServiceClient
  {
    Task<MenuOrderInfoResponseDto?> GetMenuOrderInfoAsync(string providerServiceId, string categoryId, string menuId);
  }
}
