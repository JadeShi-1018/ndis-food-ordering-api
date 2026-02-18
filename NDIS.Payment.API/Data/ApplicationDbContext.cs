using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Domain;

namespace NDIS.Payment.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.Payment> Payments { get; set; }
        public DbSet<PaymentEvent> PaymentEvents { get; set; }
    }
}