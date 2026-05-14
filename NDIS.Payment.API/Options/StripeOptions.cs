namespace NDIS.Payment.API.Options
{
  public class StripeOptions
  {
    public string SecretKey { get; set; } = null!;
    public string PublishableKey { get; set; } = null!;
    public string WebhookSecret { get; set; } = null!;
  }
}
