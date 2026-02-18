using Microsoft.EntityFrameworkCore;
using NDISS.UserRebate.API.Data;
using NDISS.UserRebate.API.Domain;

namespace NDISS.UserRebate.API.Repository
{
    public class UserRebateRepository : IUserRebateRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRebateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserRebateAsync(Domain.UserRebate userRebate)
        {
            await _context.UserRebates.AddAsync(userRebate);
            await _context.SaveChangesAsync();
        }

        public async Task<Domain.UserRebate?> GetUserRebateByUserIdAsync(string userId)
        {
            return await _context.UserRebates
                .Include(ur => ur.RebateEvents)
                .FirstOrDefaultAsync(ur => ur.UserId == userId);
        }
    }
}
