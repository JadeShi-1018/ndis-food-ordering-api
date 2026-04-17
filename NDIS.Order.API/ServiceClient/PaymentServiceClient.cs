using NDIS.Order.API.Dtos;
using System.Net.Http.Json;

namespace NDIS.Order.API.ServiceClient
{
  public class PaymentServiceClient : IPaymentServiceClient
  {
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentServiceClient> _logger;

    public PaymentServiceClient(HttpClient httpClient, ILogger<PaymentServiceClient> logger)
    {
      _httpClient = httpClient;
      _logger = logger;
    }

    public async Task<CreatePaymentResponseDto> CreatePaymentAsync(CreatePaymentRequestDto request)
    {
      var response = await _httpClient.PostAsJsonAsync("/api/payment/create", request);

      if (!response.IsSuccessStatusCode)
      {
        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError("Payment service returned error: {StatusCode}, {Error}", response.StatusCode, error);
        throw new Exception($"Payment service call failed: {response.StatusCode}");
      }

      var result = await response.Content.ReadFromJsonAsync<CreatePaymentResponseDto>();

      if (result == null)
      {
        throw new Exception("Payment service returned empty response.");
      }

      return result;
    }
  }
}