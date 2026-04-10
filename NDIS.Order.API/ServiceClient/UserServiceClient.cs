using NDIS.Order.API.Dtos;
using NDIS.Order.API.ServiceClient;
using NDIS.Shared.Common.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;


public class UserServiceClient : IUserServiceClient
{
  private readonly HttpClient _httpClient;

  public UserServiceClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<UserProfileDto?> GetUserByIdAsync(string token)
  {
    //var client = _httpClientFactory.CreateClient("UserService");

    _httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);

    var response = await _httpClient.GetAsync("api/user/me");

    if (!response.IsSuccessStatusCode)
    {
      return null;
    }

    var content = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<ApiResponse<UserProfileDto>>(content,
        new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });

    if (apiResponse == null || !apiResponse.Succeed || apiResponse.Data == null)
    {
      return null;
    }

    return apiResponse.Data;
  }
  }