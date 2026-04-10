using Microsoft.EntityFrameworkCore;
using NDIS.Order.API.DataAccess;
using NDIS.Order.API.Domain.Entities;
using OrderEntity = NDIS.Order.API.Domain.Entities.Order;
namespace NDIS.Order.API.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
      _context = context;
    }

    public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
    {
      await _context.Orders.AddAsync(order);
      return order;
    }

    public async Task<OrderEntity?> GetOrderByIdAsync(string orderId)
    {
      return await _context.Orders
          .Include(o => o.OrderEvents)
          .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }

    public async Task<List<OrderEntity>> GetOrdersByUserIdAsync(string userId)
    {
      return await _context.Orders
          .Where(o => o.UserId == userId)
          .OrderByDescending(o => o.CreatedAt)
          .ToListAsync();
    }

    public async Task<List<OrderEntity>> GetOrdersByProviderIdAsync(string providerId)
    {
      return await _context.Orders
          .Where(o => o.ProviderId == providerId)
          .OrderByDescending(o => o.CreatedAt)
          .ToListAsync();
    }

    public async Task<bool> UpdateOrderAsync(OrderEntity order)
    {
      _context.Orders.Update(order);
      return await _context.SaveChangesAsync() > 0;
    }
  }
}