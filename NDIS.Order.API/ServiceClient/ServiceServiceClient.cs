
using NDIS.Order.API.Dtos;
using NDIS.Order.API.ServiceClient;
using NDIS.Shared.Common.Models;
using System.Net.Http;
using System.Text.Json;

public class ServiceServiceClient : IServiceServiceClient
{
  private readonly HttpClient _httpClient;

  public ServiceServiceClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<MenuOrderInfoResponseDto?> GetMenuOrderInfoAsync(string providerServiceId, string categoryId, string menuId)
  {
    
    var url = $"api/ProviderService/{providerServiceId}/Categories/{categoryId}/Menu/{menuId}/order-info";

    var response = await _httpClient.GetAsync(url);
    var body = await response.Content.ReadAsStringAsync();

    //Console.WriteLine("Service API raw response: {Body}", body);
    Console.WriteLine($"Raw response body: {body}");

    if (!response.IsSuccessStatusCode)
    {
      throw new Exception(
          $"Service API call failed. StatusCode={(int)response.StatusCode}, Body={body}");
    }

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var apiResponse = JsonSerializer.Deserialize<ApiResponse<MenuOrderInfoResponseDto>>(body, options);

    if (apiResponse == null)
    {
      throw new Exception("Failed to deserialize Service API response.");
    }

    if (!apiResponse.Succeed)
    {
      throw new Exception(apiResponse.ErrorMessage ?? "Service API returned failure.");
    }

    if (apiResponse.Data == null)
    {
      throw new Exception("Service API returned success but Data was null.");
    }

    return apiResponse.Data;

    
  }

}