using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NDISS.UserRebate.API.Domain;

namespace NDIS.User.API.DbContexts
{
    public class UserRebateDbContext : DbContext
    {
        public UserRebateDbContext(DbContextOptions<UserRebateDbContext> options) : base(options) { }

        public DbSet<RebateEvent> RebateEvents { get; set; }
        public DbSet<RebateRetrySchedule> RebateRetrySchedules { get; set; }
        public DbSet<UserRebate> UserRebates { get; set; }
        public DbSet<RebateEventType> RebateEventTypes { get; set; }
        
    }
}