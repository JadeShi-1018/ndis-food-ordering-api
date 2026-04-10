using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NDIS.User.API.DbContexts;
using NDIS.User.API.Domain.User;
using AppUser = NDIS.User.API.Domain.User.User;
namespace NDIS.User.API.Data
{
  public static class UserSeeder
  {
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
      using var scope = serviceProvider.CreateScope();

      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      await dbContext.Database.MigrateAsync();

      await SeedRolesAsync(roleManager);
      await SeedNormalUsersAsync(userManager);
      await SeedProvidersAsync(userManager, dbContext);
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
      var roles = new[] { "User", "Provider" };

      foreach (var roleName in roles)
      {
        var exists = await roleManager.RoleExistsAsync(roleName);
        if (!exists)
        {
          await roleManager.CreateAsync(new Role
          {
            Name = roleName
          });
        }
      }
    }

    private static async Task SeedNormalUsersAsync(UserManager<AppUser> userManager)
    {
      var email = "user1@test.com";
      Console.WriteLine("start to seed");

      var existingUser = await userManager.FindByEmailAsync(email);
      if (existingUser != null)
      {
        return;
      }

      var user = new AppUser
      {
        UserName = email,
        Email = email,
        PhoneNumber = "0400000001",
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      var result = await userManager.CreateAsync(user, "Password123!");

      if (!result.Succeeded)
      {
        throw new Exception($"Failed to seed normal user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
      }

      await userManager.AddToRoleAsync(user, "User");
    }

    private static async Task SeedProvidersAsync(
        UserManager<AppUser> userManager,
        ApplicationDbContext dbContext)
    {
      var providerSeeds = GetProviderSeeds();

      foreach (var seed in providerSeeds)
      {
        Console.WriteLine($"Start seeding provider user: {seed.Email}");

        var existingUser = await userManager.FindByEmailAsync(seed.Email);
        if (existingUser != null)
        {
          // 确保已有 user 至少有 Provider role
          if (!await userManager.IsInRoleAsync(existingUser, "Provider"))
          {
            await userManager.AddToRoleAsync(existingUser, "Provider");
          }

          var existingProvider = await dbContext.Providers
              .FirstOrDefaultAsync(p => p.UserId == existingUser.Id);

          if (existingProvider != null)
          {
            continue;
          }

          var provider = new Provider
          {
            ProviderId = Guid.NewGuid().ToString(),
            UserId = existingUser.Id,
            ProviderName = seed.ProviderName,
            ProviderStatus = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
          };

          dbContext.Providers.Add(provider);

          dbContext.ProviderDetails.Add(new ProviderDetail
          {
            ProviderDetailId = Guid.NewGuid().ToString(),
            ProviderId = provider.ProviderId,
            CompanyName = seed.CompanyName,
            ProviderEmail = seed.Email,
            ProviderPhoneNumber = seed.PhoneNumber,
            ABN = seed.ABN,
            AddressLine = seed.AddressLine,
            City = seed.City,
            State = seed.State,
            ProviderQualification = seed.ProviderQualification
          });

          continue;
        }

        var user = new AppUser
        {
          UserName = seed.Email,
          Email = seed.Email,
          PhoneNumber = seed.PhoneNumber,
          EmailConfirmed = true,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        var createUserResult = await userManager.CreateAsync(user, "Password123!");

        if (!createUserResult.Succeeded)
        {
          throw new Exception($"Failed to seed provider user {seed.Email}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
        }

        await userManager.AddToRoleAsync(user, "Provider");

        var providerEntity = new Provider
        {
          ProviderId = Guid.NewGuid().ToString(),
          UserId = user.Id,
          ProviderName = seed.ProviderName,
          ProviderStatus = "Active",
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        dbContext.Providers.Add(providerEntity);

        dbContext.ProviderDetails.Add(new ProviderDetail
        {
          ProviderDetailId = Guid.NewGuid().ToString(),
          ProviderId = providerEntity.ProviderId,
          CompanyName = seed.CompanyName,
          ProviderEmail = seed.Email,
          ProviderPhoneNumber = seed.PhoneNumber,
          ABN = seed.ABN,
          AddressLine = seed.AddressLine,
          City = seed.City,
          State = seed.State,
          ProviderQualification = seed.ProviderQualification
        });
      }

      await dbContext.SaveChangesAsync();
    }

    private static List<ProviderSeedModel> GetProviderSeeds()
    {
      return new List<ProviderSeedModel>
            {
                new ProviderSeedModel
                {
                    Email = "provider1@test.com",
                    PhoneNumber = "0400001001",
                    ProviderName = "Healthy Meals Box Hill",
                    CompanyName = "Healthy Meals Pty Ltd",
                    ABN = "12345678901",
                    AddressLine = "12 Station St",
                    City = "Box Hill",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider2@test.com",
                    PhoneNumber = "0400001002",
                    ProviderName = "Fresh Care Kitchen Glen Waverley",
                    CompanyName = "Fresh Care Kitchen Pty Ltd",
                    ABN = "12345678902",
                    AddressLine = "25 Kingsway",
                    City = "Glen Waverley",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider3@test.com",
                    PhoneNumber = "0400001003",
                    ProviderName = "EastCare Support Meals",
                    CompanyName = "EastCare Support Pty Ltd",
                    ABN = "12345678903",
                    AddressLine = "88 Whitehorse Rd",
                    City = "Balwyn",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider4@test.com",
                    PhoneNumber = "0400001004",
                    ProviderName = "Daily Support Kitchen Richmond",
                    CompanyName = "Daily Support Kitchen Pty Ltd",
                    ABN = "12345678904",
                    AddressLine = "31 Swan St",
                    City = "Richmond",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider5@test.com",
                    PhoneNumber = "0400001005",
                    ProviderName = "WestCare Meal Services",
                    CompanyName = "WestCare Meal Services Pty Ltd",
                    ABN = "12345678905",
                    AddressLine = "100 Main Rd",
                    City = "St Albans",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider6@test.com",
                    PhoneNumber = "0400001006",
                    ProviderName = "South Yarra Care Meals",
                    CompanyName = "South Yarra Care Pty Ltd",
                    ABN = "12345678906",
                    AddressLine = "15 Toorak Rd",
                    City = "South Yarra",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider7@test.com",
                    PhoneNumber = "0400001007",
                    ProviderName = "Footscray Community Kitchen",
                    CompanyName = "Footscray Community Kitchen Pty Ltd",
                    ABN = "12345678907",
                    AddressLine = "77 Barkly St",
                    City = "Footscray",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider8@test.com",
                    PhoneNumber = "0400001008",
                    ProviderName = "Northside Fresh Meals",
                    CompanyName = "Northside Fresh Meals Pty Ltd",
                    ABN = "12345678908",
                    AddressLine = "210 High St",
                    City = "Preston",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider9@test.com",
                    PhoneNumber = "0400001009",
                    ProviderName = "CareBites Carlton",
                    CompanyName = "CareBites Carlton Pty Ltd",
                    ABN = "12345678909",
                    AddressLine = "9 Lygon St",
                    City = "Carlton",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                },
                new ProviderSeedModel
                {
                    Email = "provider10@test.com",
                    PhoneNumber = "0400001010",
                    ProviderName = "Sunshine Disability Meals",
                    CompanyName = "Sunshine Disability Meals Pty Ltd",
                    ABN = "12345678910",
                    AddressLine = "55 Hampshire Rd",
                    City = "Sunshine",
                    State = "VIC",
                    ProviderQualification = "NDIS Meal Provider"
                }
            };
    }

    private class ProviderSeedModel
    {
      public string Email { get; set; } = null!;
      public string PhoneNumber { get; set; } = null!;
      public string ProviderName { get; set; } = null!;
      public string CompanyName { get; set; } = null!;
      public string ABN { get; set; } = null!;
      public string AddressLine { get; set; } = null!;
      public string City { get; set; } = null!;
      public string State { get; set; } = null!;
      public string ProviderQualification { get; set; } = null!;
    }
  }
}