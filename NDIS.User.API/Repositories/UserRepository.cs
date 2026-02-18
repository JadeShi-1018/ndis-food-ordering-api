using Azure.Core;
using Microsoft.AspNetCore.Identity;
using NDIS.User.API.Common.Enums;
using NDIS.User.API.Domain.User;
using NDIS.User.API.DTOs;

namespace NDIS.User.API.UserReposotory
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<Domain.User.User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserRepository(UserManager<Domain.User.User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

    }
}