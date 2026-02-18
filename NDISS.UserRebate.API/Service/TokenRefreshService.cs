using NDISS.UserRebate.API.DTOs;
using System.Text.Json;

namespace NDISS.UserRebate.API.Service
{
    public class TokenRefreshService : ITokenRefreshService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TokenRefreshService> _logger;

        // 模拟配置
        private const string TokenEndpoint = "https://mock-ndis.com/oauth/token";
        private const string ClientId = "mock-client-id";
        private const string ClientSecret = "mock-client-secret";

        public TokenRefreshService(IHttpClientFactory httpClientFactory, ILogger<TokenRefreshService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TokenResponseDto> RefreshAccessTokenAsync(string refreshToken)
        {
            var client = _httpClientFactory.CreateClient();

            var body = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", ClientId },
                { "client_secret", ClientSecret }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(body)
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Refresh token failed with status code: {Status}", response.StatusCode);
                throw new Exception("Failed to refresh token");
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenData == null || string.IsNullOrEmpty(tokenData.AccessToken))
            {
                throw new Exception("Invalid token response");
            }

            return tokenData;
        }
    }
}
