using Microsoft.AspNetCore.Identity;

namespace NDIS.User.API.Domain.User
{
    public class User : IdentityUser
    {
        //public string UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //navigate property
        public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
        public ICollection<UserEvent> userEvents { get; set; } = new List<UserEvent>();
        //public ICollection<UserRole> userRoles { get; set; } = new List<UserRole>();

        public Provider Provider { get; set; }


    }
}
