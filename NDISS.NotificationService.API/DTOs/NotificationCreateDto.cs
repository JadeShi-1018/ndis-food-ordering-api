using NDISS.NotificationService.API.Domain;
using NDISS.NotificationService.API.Enums;

namespace NDISS.NotificationService.API.DTOs
{
    public class NotificationCreateDto
    {
        public string RecipientId { get; set; }
        public string NotificationMessage { get; set; }
        public string NotificationTypeId { get; set; }
        public string OrderId { get; set; }
        public bool IsRepeat { get; set; }
        public DateTime? RepeatPeriod { get; set; }
        public NotificationMethod NotificationMethod { get; set; }
    }
}
