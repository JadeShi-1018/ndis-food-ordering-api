using Microsoft.AspNetCore.Identity;

namespace NDIS.User.API.Domain.User
{
    public class Provider
    {
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ProviderStatus { get; set; }


        //navigate properties
        public User User { get; set; }

       

    }
}
