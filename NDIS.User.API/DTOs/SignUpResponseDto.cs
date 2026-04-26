namespace NDIS.User.API.DTOs
{
  public class SignUpResponseDto
  {
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;

  }
}
