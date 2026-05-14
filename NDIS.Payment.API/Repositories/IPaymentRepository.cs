using NDIS.Payment.API.Domain;
using System.Threading.Tasks;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Repositories
{
  public interface IPaymentRepository
  {
    Task<PaymentEntity?> GetByOrderIdAsync(string orderId);

    Task<PaymentEntity?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId);

    Task AddAsync(PaymentEntity payment);

    Task UpdateAsync(PaymentEntity payment);
  }
}
