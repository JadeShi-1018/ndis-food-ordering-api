using NDISS.NotificationService.API.Domain;

namespace NDISS.NotificationService.API.Repository
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task AddNotificationEventAsync(NotificationEvent evt);
    }
}
