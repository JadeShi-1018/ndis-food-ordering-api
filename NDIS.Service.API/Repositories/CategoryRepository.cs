    using Microsoft.EntityFrameworkCore;
using NDIS.Service.API.Data;
using NDIS.Service.API.Domain;

namespace NDIS.Service.API.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetByProviderServiceIdAsync(string providerServiceId)
        {
            return await _context.Categories
                .Where(c => c.ProviderServiceId == providerServiceId)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAndProviderAsync(string categoryId, string providerServiceId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.ProviderServiceId == providerServiceId);
        }
    }
}
