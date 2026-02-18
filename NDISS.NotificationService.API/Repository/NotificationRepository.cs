using NDISS.NotificationService.API.Data;
using NDISS.NotificationService.API.Domain;
using NDISS.NotificationService.API.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddNotificationAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task AddNotificationEventAsync(NotificationEvent evt)
    {
        _context.NotificationEvents.Add(evt);
        await _context.SaveChangesAsync();
    }
}
