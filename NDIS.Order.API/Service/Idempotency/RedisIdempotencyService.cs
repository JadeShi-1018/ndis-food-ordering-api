using NDIS.Order.API.Service.Idempotency;
using StackExchange.Redis;

public class RedisIdempotencyService : IIdempotencyService
{
  private readonly IDatabase _redis;

  public RedisIdempotencyService(IConnectionMultiplexer redis)
  {
    _redis = redis.GetDatabase();
  }

  public async Task<IdempotencyCheckResult> TryStartAsync(string key, TimeSpan expiry)
  {
    // Redis: SET key PROCESSING NX EX 600
    var locked = await _redis.StringSetAsync(
        key,
        "PROCESSING",
        expiry,
        When.NotExists);

    if (locked)
    {
      return new IdempotencyCheckResult
      {
        Acquired = true
      };
    }

    var existingValue = await _redis.StringGetAsync(key);

    if (!existingValue.HasValue)
    {
      return new IdempotencyCheckResult
      {
        Acquired = false
      };
    }

    var value = existingValue.ToString();

    if (value == "PROCESSING")
    {
      return new IdempotencyCheckResult
      {
        Acquired = false,
        IsProcessing = true
      };
    }

    if (value.StartsWith("SUCCESS:"))
    {
      return new IdempotencyCheckResult
      {
        Acquired = false,
        ExistingOrderId = value.Substring("SUCCESS:".Length)
      };
    }

    return new IdempotencyCheckResult
    {
      Acquired = false
    };
  }

  public async Task ReleaseAsync(string key)
  {
    await _redis.KeyDeleteAsync(key);
  }

  public async Task MarkSuccessAsync(string key, string orderId, TimeSpan expiry)
  {
    await _redis.StringSetAsync(key, $"SUCCESS:{orderId}", expiry);
  }
}