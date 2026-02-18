using AppUser = NDIS.User.API.Domain.User.User;

namespace NDIS.User.API.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }
}
