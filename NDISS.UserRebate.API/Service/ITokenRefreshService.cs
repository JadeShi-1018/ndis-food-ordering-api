using NDISS.UserRebate.API.DTOs;

namespace NDISS.UserRebate.API.Service
{
    public interface ITokenRefreshService
    {
        Task<TokenResponseDto> RefreshAccessTokenAsync(string refreshToken);
    }
}
