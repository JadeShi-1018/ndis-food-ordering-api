namespace NDISS.NotificationService.API.Domain
{
    public class NotificationType
    {
        public string NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }

        // Navigation
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
