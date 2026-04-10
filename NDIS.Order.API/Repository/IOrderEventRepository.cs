using NDIS.Order.API.Domain.Entities;

namespace NDIS.Order.API.Repository
{
  public interface IOrderEventRepository
  {

    Task AddAsync(OrderEvent orderEvent);
    Task<List<OrderEvent>> GetPendingEventsAsync(int batchSize);
    Task SaveChangesAsync();
  }
}
