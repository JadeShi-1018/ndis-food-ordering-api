using Microsoft.EntityFrameworkCore;
using NDISS.NotificationService.API.Domain;

namespace NDISS.NotificationService.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationEvent> NotificationEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NotificationType>().HasData(
                new NotificationType { NotificationTypeId = "1", NotificationTypeName = "SMS" },
                new NotificationType { NotificationTypeId = "2", NotificationTypeName = "Email" }
            );

        }
    }
}
