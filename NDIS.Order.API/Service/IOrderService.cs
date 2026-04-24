using NDIS.Order.API.Dtos;
using StackExchange.Redis;
using System.Security.Claims;

namespace NDIS.Order.API.Services
{
  public interface IOrderService
  {
    Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request, ClaimsPrincipal user);
    Task<OrderResponseDto?> GetOrderByIdAsync(string orderId);
    Task<List<OrderResponseDto>> GetOrdersByUserIdAsync(string userId);

    Task<bool> UpdateOrderStatusAsync(string orderId, string orderStatus);

    Task<List<OrderResponseDto>> GetMyOrdersAsync(ClaimsPrincipal user);
  }
}