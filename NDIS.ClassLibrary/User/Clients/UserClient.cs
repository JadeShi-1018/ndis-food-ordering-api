using NDIS.Shared.Common.Models;
using NDIS.Shared.User.DTOs;
using NDIS.Shared.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NDIS.Shared.User.Clients
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<UserDto>> GetUserInfoAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    return ApiResponse<UserDto>.Success(user!, "User info fetched successfully");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return ApiResponse<UserDto>.Fail($"Server returned error: {error}", ((int)response.StatusCode).ToString());
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Fail($"Exception occurred: {ex.Message}", "CLIENT_ERROR");
            }
        }
    }
}
