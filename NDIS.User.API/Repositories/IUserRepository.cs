using Microsoft.AspNetCore.Identity;
using NDIS.User.API.DTOs;
using AppUser = NDIS.User.API.Domain.User.User;

namespace NDIS.User.API.UserReposotory
{
    public interface IUserRepository
    {
    Task<AppUser?> GetByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(AppUser user, string password);
    Task<IdentityResult> AddToRoleAsync(AppUser user, string role);

  }
}
