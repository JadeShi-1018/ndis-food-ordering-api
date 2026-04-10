using Microsoft.EntityFrameworkCore;
using NDIS.Order.API.Domain.Entities;
using OrderEntity = NDIS.Order.API.Domain.Entities.Order;

namespace NDIS.Order.API.DataAccess
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderEvent> OrderEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<OrderEntity>(entity =>
      {
        entity.HasKey(o => o.OrderId);

        entity.HasIndex(o => new { o.UserId, o.IdempotencyKey })
    .IsUnique();

        entity.Property(o => o.UserId)
              .IsRequired();

        entity.Property(o => o.CustomerName)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(o => o.ProviderId)
              .IsRequired();

        entity.Property(o => o.ProviderServiceName)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(o => o.ProviderServiceId)
              .IsRequired();

    

        entity.Property(o => o.MenuId)
              .IsRequired();

        entity.Property(o => o.MenuName)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(o => o.PeriodName)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(o => o.Quantity)
              .IsRequired();

        entity.Property(o => o.UnitPrice)
              .IsRequired()
              .HasColumnType("decimal(18,2)");

        entity.Property(o => o.OrderPrice)
              .IsRequired()
              .HasColumnType("decimal(18,2)");

        entity.Property(o => o.DeliveryAddress)
              .IsRequired()
              .HasMaxLength(255);

        entity.Property(o => o.CustomerContactNumber)
              .IsRequired()
              .HasMaxLength(30);

        modelBuilder.Entity<OrderEntity>()
           .Property(o => o.OrderStatus)
           .HasConversion<string>();

        entity.Property(o => o.StartDate)
              .IsRequired();

        entity.Property(o => o.OrderStatus)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(o => o.CreatedAt)
              .IsRequired();

        entity.Property(o => o.UpdatedAt)
              .IsRequired();
      });

      modelBuilder.Entity<OrderEvent>(entity =>
      {
        entity.HasKey(e => e.OrderEventId);

        entity.Property(e => e.EventType)
            .HasConversion<int>();

        entity.Property(e => e.EventStatus)
            .HasConversion<int>();

        entity.Property(e => e.Payload)
            .IsRequired();

        entity.Property(e => e.ErrorMessage)
            .HasMaxLength(2000);

        entity.Property(e => e.EventTimestamp)
            .IsRequired();

        entity.HasIndex(e => e.EventStatus);
        entity.HasIndex(e => e.EventTimestamp);

        entity.HasOne(e => e.Order)
            .WithMany(o => o.OrderEvents)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
      });
    }

  }
 

}
