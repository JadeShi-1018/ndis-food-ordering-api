using Microsoft.EntityFrameworkCore;
using NDIS.Order.API.DataAccess;
using NDIS.Order.API.Domain.Entities;
using NDIS.Order.API.Domain.Enums;

namespace NDIS.Order.API.Repository
{
  public class OrderEventRepository : IOrderEventRepository
  {
    private readonly OrderDbContext _context;

    public OrderEventRepository(OrderDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(OrderEvent orderEvent)
    {
      await _context.OrderEvents.AddAsync(orderEvent);
    }

    public async Task<List<OrderEvent>> GetPendingEventsAsync(int batchSize)
    {
      return await _context.OrderEvents
          .Where(x => x.EventStatus == OrderEventStatus.Pending)
          .OrderBy(x => x.EventTimestamp)
          .Take(batchSize)
          .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
