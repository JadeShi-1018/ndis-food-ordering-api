namespace NDIS.User.API.DTOs
{
    public class SignUpRequestDto
    {
    public string UserName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        
    }
}
