using Microsoft.EntityFrameworkCore;
using NDISS.UserRebate.API.Domain;

namespace NDISS.UserRebate.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.UserRebate> UserRebates { get; set; }
        public DbSet<RebateEvent> RebateEvents { get; set; }
        public DbSet<RebateEventType> RebateEventTypes { get; set; }
        public DbSet<RebateRetrySchedule> RebateRetrySchedules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RebateRetrySchedule>(entity =>
            {
                entity.Property(e => e.Status)
                      .HasConversion<string>();
            });
            modelBuilder.Entity<RebateEvent>(entity =>
            {
                entity.Property(e => e.SyncStatus)
                      .HasConversion<string>();
            });
        }

    }
}
