using NDISS.NotificationService.API.Enums;

namespace NDISS.NotificationService.API.Domain
{
    public class NotificationEvent
    {
        public string NotificationEventId { get; set; }
        public string NotificationId { get; set; }
        public NotificationMethod NotificationMethod { get; set; } //enum?
        public NotificationEventStatus EventStatus { get; set; }
        public NotificationEventType EventType { get; set; }
        public string ErrorReason { get; set; }
        public DateTime SentAt { get; set; }
        // Navigation
        public Notification Notification { get; set; }
    }
}