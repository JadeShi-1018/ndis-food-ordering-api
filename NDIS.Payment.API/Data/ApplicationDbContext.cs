using MassTransit.Futures.Contracts;
using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Domain;
using NDIS.Payment.API.Services.Outbox;
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
    public DbSet<ProcessedWebhookEvent> ProcessedWebhookEvents { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<PaymentEntity>(entity =>
      {
        entity.HasKey(x => x.PaymentId);

        entity.Property(x => x.UnitPrice)
              .HasColumnType("decimal(18,2)");

        entity.Property(x => x.PaymentPrice)
              .HasColumnType("decimal(18,2)");

        entity.Property(x => x.PaymentStatus)
              .HasConversion<string>();

        entity.Property(x => x.CreatedAt)
              .HasDefaultValueSql("GETUTCDATE()");

        entity.HasIndex(x => x.OrderId)
              .IsUnique();

        entity.HasIndex(p => p.StripePaymentIntentId);

             
      });

      modelBuilder.Entity<ProcessedWebhookEvent>()
    .HasIndex(e => new { e.Provider, e.EventId })
    .IsUnique();
      modelBuilder.Entity<OutboxMessage>()
    .HasIndex(o => o.ProcessedAt);

    }
  }
}