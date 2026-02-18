using NDIS.User.API.DTOs;
using System.Security.Claims;

namespace NDIS.User.API.UserService
{
    public interface IUserService
    {
        Task<UserInfoResponseDto> UpdateUserInfoAsync(string userId, UpdateUserRequestDto dto);
        Task<UserInfoDto> GetCurrentUserInfoAsync(string userId);
        Task<SignInResponseDto> SignInAsync(SignInRequestDto request);
        Task<SignUpResponseDto?> RegisterAsync(SignUpRequestDto signUpRequestDto, string roleName);
    }
}
