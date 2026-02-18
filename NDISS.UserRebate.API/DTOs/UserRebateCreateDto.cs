namespace NDISS.UserRebate.API.DTOs
{
    public class UserRebateCreateDto
    {
        public float RebateRate { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string RebateEventTypeId { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

}
