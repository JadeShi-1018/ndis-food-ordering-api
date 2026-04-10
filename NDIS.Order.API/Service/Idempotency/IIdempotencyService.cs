using System.Threading.Tasks;

namespace NDIS.Order.API.Service.Idempotency
{
  public interface IIdempotencyService
  {

    Task<IdempotencyCheckResult> TryStartAsync(string key, TimeSpan expiry);
    Task MarkSuccessAsync(string key, string orderId, TimeSpan expiry);
    Task ReleaseAsync(string key);
  }
}
