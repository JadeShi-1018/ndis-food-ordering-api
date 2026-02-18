using System.Text.Json;
namespace NDIS.User.API.Domain.User
{
    public class UserEvent
    {
        public string UserEventId { get; set; }
        public string EventType { get; set; }
        public string EventStatus { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        //navigate property
        
        public User User { get; set; }
    }
}
