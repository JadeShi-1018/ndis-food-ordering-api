using System.Threading.Tasks;

namespace NDIS.Order.API.Service.Idempotency
{
  public interface IIdempotencyService
  {

    Task<IdempotencyCheckResult> TryStartAsync(string key, TimeSpan expiry);
    Task<bool> MarkSuccessAsync(string key, string orderId, TimeSpan expiry,string token);
    Task<bool> ReleaseAsync(string key, string token);
  }
}
