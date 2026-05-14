using NDIS.Order.API.Domain.Entities;
using OrderEntity = NDIS.Order.API.Domain.Entities.Order;

namespace NDIS.Order.API.Repositories
{
  public interface IOrderRepository
  {
    Task<OrderEntity> CreateOrderAsync(OrderEntity order);
    Task<OrderEntity?> GetOrderByIdAsync(string orderId);
    Task<List<OrderEntity>> GetOrdersByUserIdAsync(string userId);
    Task<List<OrderEntity>> GetOrdersByProviderIdAsync(string providerId);
    Task<bool> UpdateOrderAsync(OrderEntity order);
    Task SaveChangesAsync();

  }
}