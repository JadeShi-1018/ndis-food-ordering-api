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
    // 每一次后端 request attempt 都生成一个新的 token
    var token = Guid.NewGuid().ToString("N");

    // Redis value 不再只是 PROCESSING，而是 PROCESSING:{token}
    var processingValue = $"PROCESSING:{token}";
    // Redis: SET key PROCESSING NX EX 600
    var locked = await _redis.StringSetAsync(
        key,
       processingValue,
    // Redis: SET key PROCESSING,
        expiry,
        When.NotExists);

    if (locked)
    {
      return new IdempotencyCheckResult
      {
        Acquired = true,
        Token = token
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

    if (value.StartsWith("PROCESSING"))
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

  public async Task<bool> ReleaseAsync(string key, string token)
  {
    // only when the current Redis  value == PROCESSING: { token} and then you can delete the idempotency key

    //  Lua script used to ensure actomic
    var script = @"
            if redis.call('GET', KEYS[1]) == ARGV[1] then
                return redis.call('DEL', KEYS[1])
            else
                return 0
            end";
    var result = await _redis.ScriptEvaluateAsync(script,
            new RedisKey[] { key },
            new RedisValue[] { $"PROCESSING:{token}" });

    return (int) result == 1;
  }

  public async Task<bool> MarkSuccessAsync(string key, string orderId, TimeSpan expiry,string token)
  {
    // 只有 Redis 当前 value 仍然是 PROCESSING:{token}
    // 才允许把它改成 SUCCESS:{orderId}
    //
    // 这个也必须原子化，避免旧请求覆盖新请求状态
    var script = @"
            if redis.call('GET', KEYS[1]) == ARGV[1] then
                return redis.call('SET', KEYS[1], ARGV[2], 'EX', ARGV[3])
            else
                return nil
            end";

    var result = await _redis.ScriptEvaluateAsync(
        script,
        new RedisKey[] { key },
        new RedisValue[]
        {
                $"PROCESSING:{token}",
                $"SUCCESS:{orderId}",
                (long)expiry.TotalSeconds
        });

    return !result.IsNull;

  }
}