using Microsoft.EntityFrameworkCore;
using NDIS.Order.API.Domain.Entities;

namespace NDIS.Order.API.DataAccess
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Domain.Entities.Order> Orders { get; set; }
        public DbSet<OrderEvent> OrderEvent { get; set; }

    }
 

}
