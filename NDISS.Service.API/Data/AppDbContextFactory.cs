using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NDISS.Service.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory(); // ✅ 不再拼 NDISS.Service.API

            var configPath = Path.Combine(basePath, "appsettings.json");

            Console.WriteLine($"🔧 [EF Design-Time] Loading config from: {basePath}");

            if (!File.Exists(configPath))
            {
                Console.WriteLine($"❌ appsettings.json NOT FOUND at: {configPath}");
                throw new FileNotFoundException("Could not find appsettings.json", configPath);
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("NDISSService");

            Console.WriteLine($"🔧 [EF Design-Time] Using connection string: {connectionString}");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
