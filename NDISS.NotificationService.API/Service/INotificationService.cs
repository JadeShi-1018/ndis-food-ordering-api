using NDISS.NotificationService.API.DTOs;

namespace NDISS.NotificationService.API.Service
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(NotificationCreateDto dto);
    }
}
