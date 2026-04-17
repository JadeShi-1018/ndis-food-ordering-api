using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Domain;
using PaymentEntity = NDIS.Payment.API.Domain.Payment;

namespace NDIS.Payment.API.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PaymentEntity> Payments { get; set; }
    

   
    
  }
}