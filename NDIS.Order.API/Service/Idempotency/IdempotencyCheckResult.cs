namespace NDIS.Order.API.Service.Idempotency
{
  public class IdempotencyCheckResult
  {
    public bool Acquired { get; set; }
    public bool IsProcessing { get; set; }
    public string? ExistingOrderId { get; set; }
  }
}
