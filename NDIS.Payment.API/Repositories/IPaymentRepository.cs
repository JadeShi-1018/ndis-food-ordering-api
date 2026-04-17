using NDIS.Payment.API.Domain;
using System.Threading.Tasks;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Repositories
{
  public interface IPaymentRepository
  {
  
    Task SaveChangesAsync();

    Task AddAsync(PaymentEntity payment);
    Task<PaymentEntity?> GetByIdempotencyKeyAsync(string idempotencyKey);
    Task<PaymentEntity?> GetByIdAsync(string paymentId);

  }
}
