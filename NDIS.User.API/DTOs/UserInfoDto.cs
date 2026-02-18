namespace NDIS.User.API.DTOs
{
    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public List<string> Address { get; set; }
    }
}
