using NDIS.Service.API.Domain;

namespace NDIS.Service.API.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetByProviderServiceIdAsync(string providerServiceId);
        Task<Category?> GetByIdAndProviderAsync(string categoryId, string providerServiceId);
    }
}