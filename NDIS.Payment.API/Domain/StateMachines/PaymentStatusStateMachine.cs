using NDIS.Payment.API.Enums;

namespace NDIS.Payment.API.Domain.StateMachines
{
  public class PaymentStatusStateMachine
  {
    public static bool CanTransition(PaymentStatus current, PaymentStatus next)
    {
      return current switch
      {
        PaymentStatus.Pending => next is PaymentStatus.Succeeded
            or PaymentStatus.Failed,

        PaymentStatus.Succeeded => false,
        PaymentStatus.Failed => false,

        _ => false
      };
      }
      }
}
