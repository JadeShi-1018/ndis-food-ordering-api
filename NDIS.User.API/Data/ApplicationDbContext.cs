using User = NDIS.User.API.Domain.User.User;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using NDIS.User.API.Domain.User;
//using User = NDIS.User.API.Models.User;
namespace NDIS.User.API.DbContexts
{


        public class ApplicationDbContext : IdentityDbContext<Domain.User.User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets for your models
        //public DbSet<NDIS.User.API.Models.User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
            public DbSet<UserAddress> UserAddresses { get; set; }
            public DbSet<Provider> Providers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

            // Configure UserEvent relationship
            // 1:1 → User ↔ Provider
            modelBuilder.Entity<Domain.User.User>()
                .HasOne(u => u.Provider)
                .WithOne(p => p.User)
                .HasForeignKey<Provider>(p => p.ProviderId);

            // 1:N → User → UserAddresses
            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAddresses)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N → User → UserEvents
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.userEvents)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
