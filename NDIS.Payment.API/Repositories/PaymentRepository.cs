using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Data;
using NDIS.Payment.API.Domain;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Repositories
{
  public class PaymentRepository : IPaymentRepository
  {

    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<PaymentEntity?> GetByOrderIdAsync(string orderId)
    {
      return await _context.Payments
          .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<PaymentEntity?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId)
    {
      return await _context.Payments
          .FirstOrDefaultAsync(p => p.StripePaymentIntentId == stripePaymentIntentId);
    }

    public async Task AddAsync(PaymentEntity payment)
    {
      await _context.Payments.AddAsync(payment);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentEntity payment)
    {
      payment.UpdatedAt = DateTime.UtcNow;

      _context.Payments.Update(payment);
      await _context.SaveChangesAsync();
    }
  }
}
