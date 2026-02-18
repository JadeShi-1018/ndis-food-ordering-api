using NDISS.UserRebate.API.DTOs;

namespace NDISS.UserRebate.API.Service
{
    public interface IUserRebateService
    {
        Task SaveUserRebateAndEventAsync(string userId, UserRebateCreateDto dto);
    }
}
