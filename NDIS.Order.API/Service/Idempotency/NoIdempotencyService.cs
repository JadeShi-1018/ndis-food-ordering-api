
    using NDIS.Order.API.Service.Idempotency;

public class NoOpIdempotencyService : IIdempotencyService
  {
    public Task<IdempotencyCheckResult> TryStartAsync(string key, TimeSpan expiry)
    {
      return Task.FromResult(new IdempotencyCheckResult
      {
        Acquired = true
      });
    }

    public Task ReleaseAsync(string key)
    {
      return Task.CompletedTask;
    }

    public Task MarkSuccessAsync(string key, string orderId, TimeSpan expiry)
    {
      return Task.CompletedTask;
    }
  
}

