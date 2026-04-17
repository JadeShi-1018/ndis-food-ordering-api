using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Data;
using NDIS.Payment.API.Domain;
using NDIS.Payment.API.Dtos;
using NDIS.Payment.API.Repositories;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Repository
{
  public class PaymentRepository : IPaymentRepository
  {
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task AddAsync(PaymentEntity payment)
    {
      await _context.Payments.AddAsync(payment);
    }

    public async Task<PaymentEntity?> GetByIdempotencyKeyAsync(string idempotencyKey)
    {
      return await _context.Payments
          .FirstOrDefaultAsync(p => p.IdempotencyKey == idempotencyKey);
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }

    public async Task<PaymentEntity?> GetByIdAsync(string paymentId)
    {
      return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    
  }
}