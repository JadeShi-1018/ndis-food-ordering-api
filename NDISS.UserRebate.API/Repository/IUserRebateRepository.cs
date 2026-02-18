using NDISS.UserRebate.API.Domain;

namespace NDISS.UserRebate.API.Repository
{
    public interface IUserRebateRepository
    {
        Task AddUserRebateAsync(Domain.UserRebate userRebate);
        Task<Domain.UserRebate> GetUserRebateByUserIdAsync(string userId);
    }
}
