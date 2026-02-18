using Microsoft.EntityFrameworkCore;

namespace NDISS.Service.API.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(string id);

        Task AddAsync(T entity);

        Task Update(T entity);

        Task DeleteAsync(string id);

        Task SaveAsync();

        DbSet<TType> GetDbSet<TType>() where TType : class;
    }
}
