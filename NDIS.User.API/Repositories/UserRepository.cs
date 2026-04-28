using Azure.Core;
using Microsoft.AspNetCore.Identity;
using NDIS.User.API.Common.Enums;
using NDIS.User.API.Domain.User;
using NDIS.User.API.DTOs;

using AppUser = NDIS.User.API.Domain.User.User;

namespace NDIS.User.API.UserReposotory
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
       

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
      return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateAsync(AppUser user, string password)
    {
      return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddToRoleAsync(AppUser user, string role)
    {
      return await _userManager.AddToRoleAsync(user, role);

    }

    }
}