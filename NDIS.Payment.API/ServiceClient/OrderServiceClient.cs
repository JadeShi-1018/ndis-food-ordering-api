using System.Net.Http.Json;
using NDIS.Payment.API.Dtos;

namespace NDIS.Payment.API.ServiceClient
{
  public class OrderServiceClient : IOrderServiceClient
  {
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderServiceClient> _logger;

    public OrderServiceClient(HttpClient httpClient, ILogger<OrderServiceClient> logger)
    {
      _httpClient = httpClient;
      _logger = logger;
    }

    public async Task UpdateOrderStatusAsync(string orderId, UpdateOrderStatusRequestDto request)
    {
      var response = await _httpClient.PutAsJsonAsync($"/api/order/{orderId}/status", request);

      if (!response.IsSuccessStatusCode)
      {
        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError(
            "Order service update status failed. OrderId: {OrderId}, StatusCode: {StatusCode}, Body: {Body}",
            orderId,
            response.StatusCode,
            error);

        throw new Exception($"Order service update failed: {response.StatusCode}");
      }
    }
  }
}