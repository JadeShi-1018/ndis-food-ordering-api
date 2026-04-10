using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NDIS.Order.API.DataAccess;

namespace NDIS.Order.API
{
  public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
  {
    public OrderDbContext CreateDbContext(string[] args)
    {
      var basePath = Directory.GetCurrentDirectory();

      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(basePath)
          .AddJsonFile("appsettings.json", optional: false)
          .Build();

      var connectionString = configuration.GetConnectionString("NDISOrderService");

      var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
      optionsBuilder.UseSqlServer(connectionString);

      return new OrderDbContext(optionsBuilder.Options);
    }
  }
}