using Microsoft.EntityFrameworkCore;
using NDISS.Service.API.Domain;

namespace NDISS.Service.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProviderService> ProviderServices => Set<ProviderService>();
        public DbSet<ProviderServiceLocation> ProviderServiceLocations => Set<ProviderServiceLocation>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Period> Periods => Set<Period>();
        public DbSet<WeekDay> WeekDays => Set<WeekDay>();
        public DbSet<WeeklyPlan> WeeklyPlans => Set<WeeklyPlan>();
        public DbSet<SinglePlan> SinglePlans => Set<SinglePlan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.Menu)
                .WithMany(m => m.MenuItems)
                .HasForeignKey(mi => mi.MenuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.Item)
                .WithMany(i => i.MenuItems)
                .HasForeignKey(mi => mi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SinglePlan>()
                .HasOne(sp => sp.Menu)
                .WithMany()
                .HasForeignKey(sp => sp.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SinglePlan>()
                .HasOne(sp => sp.WeeklyPlan)
                .WithMany(wp => wp.SinglePlans)
                .HasForeignKey(sp => sp.WeeklyPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
