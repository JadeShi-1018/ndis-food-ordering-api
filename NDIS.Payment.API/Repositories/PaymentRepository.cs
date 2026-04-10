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
          .Include(x => x.PaymentEvents)
          .FirstOrDefaultAsync(x => x.OrderId == orderId);
    }

    public async Task AddAsync(PaymentEntity payment)
    {
      await _context.Payments.AddAsync(payment);
    }

    public async Task AddPaymentEventAsync(PaymentEvent paymentEvent)
    {
      await _context.PaymentEvents.AddAsync(paymentEvent);
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
