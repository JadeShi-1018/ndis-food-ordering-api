using NDIS.Shared.Common.Models;
using NDIS.Shared.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDIS.Shared.User.Interfaces
{
    public interface IUserClient
    {
        Task<ApiResponse<UserDto>> GetUserInfoAsync(string userId);
    }
}
