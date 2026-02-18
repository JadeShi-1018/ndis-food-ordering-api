using Microsoft.EntityFrameworkCore;
using NDISS.UserRebate.API.Data;
using NDISS.UserRebate.API.Domain;

namespace NDISS.UserRebate.API.Repository
{
    public class RebateEventRepository : IRebateEventRepository
    {
        private readonly ApplicationDbContext _context;
        public RebateEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRebateEventAsync(RebateEvent rebateEvent)
        {
            await _context.RebateEvents.AddAsync(rebateEvent);
            await _context.SaveChangesAsync();
        }
    }
}
