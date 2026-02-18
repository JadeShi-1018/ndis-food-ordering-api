using NDISS.NotificationService.API.Domain;

namespace NDISS.NotificationService.API.Domain
{
    public class Notification
    {
        public string NotificationId { get; set; }
        public string RecipientId { get; set; }
        public string NotificationMessage { get; set; }
        public string NotificationTypeId { get; set; }
        public string OrderId { get; set; }
        public bool IsRepeat { get; set; }
        public DateTime? RepeatPeriod { get; set; }
        public DateTime CreatedAt { get; set; }
        // Navigation
        public NotificationType NotificationType { get; set; }
        public ICollection<NotificationEvent> NotificationEvents { get; set; } = new List<NotificationEvent>();
    }
}