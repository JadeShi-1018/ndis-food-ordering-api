using NDISS.UserRebate.API.Domain;

namespace NDISS.UserRebate.API.Repository
{
    public interface IRebateEventRepository
    {
        Task AddRebateEventAsync(RebateEvent rebateEvent);
    }
}
