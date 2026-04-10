using NDISS.Service.API.Domain;

namespace NDISS.Service.API.Data
{
  public class AppDbSeeder
  {
    public static async Task SeedAsync(AppDbContext context)
    {
      if (!context.ServiceTypes.Any())
      {
        var serviceTypes = new List<ServiceType>
                {
                    new ServiceType
                    {
                        ServiceTypeId = Guid.NewGuid().ToString(),
                        ServiceTypeName = "Meal Delivery",
                        ServiceDescription = "Prepared meal delivery service for NDIS participants."
                    },
                    new ServiceType
                    {
                        ServiceTypeId = Guid.NewGuid().ToString(),
                        ServiceTypeName = "Home Care",
                        ServiceDescription = "In-home support and personal care services."
                    }
                };

        context.ServiceTypes.AddRange(serviceTypes);
        await context.SaveChangesAsync();
      }

      if (!context.ProviderServices.Any())
      {
        var mealDeliveryType = context.ServiceTypes
            .FirstOrDefault(st => st.ServiceTypeName == "Meal Delivery");

        if (mealDeliveryType == null)
        {
          throw new Exception("Meal Delivery service type not found.");
        }

        var providerServices = new List<ProviderService>
                {
                    new ProviderService
                    {
                        ProviderServiceId = Guid.NewGuid().ToString(),
                        ProviderServiceName = "Kim Kitchen Box Hill",
                        ProviderId = SeedConstants.Provider2Id,
                        ServiceTypeId = mealDeliveryType.ServiceTypeId,
                        PhoneNumber = "0400000001",
                        OpeningHours = "Mon-Sat 9:00-18:00",
                        Address = "12 Station St",
                        City = "Box Hill",
                        State = "VIC",
                        Postcode = 3128,
                        Lat = -37.8189,
                        Long = 145.1253
                    },
                    new ProviderService
                    {
                        ProviderServiceId = Guid.NewGuid().ToString(),
                        ProviderServiceName = "Healthy Bites Glen Waverley",
                        ProviderId = SeedConstants.Provider1Id,
                        ServiceTypeId = mealDeliveryType.ServiceTypeId,
                        PhoneNumber = "0400000002",
                        OpeningHours = "Mon-Sun 8:00-19:00",
                        Address = "88 Kingsway",
                        City = "Glen Waverley",
                        State = "VIC",
                        Postcode = 3150,
                        Lat = -37.8794,
                        Long = 145.1647
                    }

      };

        context.ProviderServices.AddRange(providerServices);
        await context.SaveChangesAsync();
      }

      if (!context.Categories.Any())
      {
        var boxHillService = context.ProviderServices
            .FirstOrDefault(ps => ps.ProviderServiceName == "Kim Kitchen Box Hill");

        var glenService = context.ProviderServices
            .FirstOrDefault(ps => ps.ProviderServiceName == "Healthy Bites Glen Waverley");

        if (boxHillService == null || glenService == null)
        {
          throw new Exception("Provider services not found.");
        }

        var categories = new List<Category>
    {
        new Category
        {
            CategoryId = Guid.NewGuid().ToString(),
            CategoryName = "Vegetarian",
            CategoryDescription = "Healthy vegetarian meals with balanced nutrition.",
            ProviderServiceId = boxHillService.ProviderServiceId
        },
        new Category
        {
            CategoryId = Guid.NewGuid().ToString(),
            CategoryName = "Meat Dishes",
            CategoryDescription = "Protein-rich meat meals suitable for daily energy needs.",
            ProviderServiceId = boxHillService.ProviderServiceId
        },
        new Category
        {
            CategoryId = Guid.NewGuid().ToString(),
            CategoryName = "Low Sugar",
            CategoryDescription = "Meals suitable for participants who prefer lower sugar options.",
            ProviderServiceId = glenService.ProviderServiceId
        },
        new Category
        {
            CategoryId = Guid.NewGuid().ToString(),
            CategoryName = "Soft Texture",
            CategoryDescription = "Easy-to-digest meals with soft texture, suitable for elderly users.",
            ProviderServiceId = glenService.ProviderServiceId
        }
    };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
      }

      if (!context.Menus.Any())
      {
        var vegetarianCategory = context.Categories
            .FirstOrDefault(c => c.CategoryName == "Vegetarian");

        var meatCategory = context.Categories
            .FirstOrDefault(c => c.CategoryName == "Meat Dishes");

        var lowSugarCategory = context.Categories
            .FirstOrDefault(c => c.CategoryName == "Low Sugar");

        var softTextureCategory = context.Categories
            .FirstOrDefault(c => c.CategoryName == "Soft Texture");

        if (vegetarianCategory == null || meatCategory == null || lowSugarCategory == null || softTextureCategory == null)
        {
          throw new Exception("One or more categories not found.");
        }

        var menus = new List<Menu>
    {
        new Menu
        {
            MenuId = Guid.NewGuid().ToString(),
            MenuName = "Tomato Scrambled Eggs",
            Description = "Soft texture, light seasoning, balanced protein, suitable for daily meals.",
            Period = MenuPeriod.Daily,
            Price = 14.50m,
            CategoryId = vegetarianCategory.CategoryId
        },
        new Menu
        {
            MenuId = Guid.NewGuid().ToString(),
            MenuName = "Vegetable Tofu Bowl",
            Description = "High-protein vegetarian meal with tofu and seasonal vegetables, easy to digest.",
            Period = MenuPeriod.Daily,
            Price = 15.90m,
            CategoryId = vegetarianCategory.CategoryId
        },
        new Menu
        {
            MenuId = Guid.NewGuid().ToString(),
            MenuName = "Braised Chicken with Rice",
            Description = "Tender chicken with steamed rice, high in protein and suitable for daily nutrition needs.",
            Period = MenuPeriod.Daily,
            Price = 16.90m,
            CategoryId = meatCategory.CategoryId
        },
        new Menu
        {
            MenuId = Guid.NewGuid().ToString(),
            MenuName = "Beef and Pumpkin Meal",
            Description = "Soft beef slices with pumpkin, rich in protein and suitable for users who prefer warm meals.",
            Period = MenuPeriod.Daily,
            Price = 17.50m,
            CategoryId = meatCategory.CategoryId
        },
        new Menu
        {
            MenuId = Guid.NewGuid().ToString(),
            MenuName = "Low Sugar Weekly Meal Pack",
            Description = "A weekly meal pack designed for participants who prefer lower sugar and balanced nutrition.",
            Period = MenuPeriod.Weekly,
            Price = 89.00m,
            CategoryId = lowSugarCategory.CategoryId
        },
        new Menu
        {
            MenuId = Guid.NewGuid().ToString(),
            MenuName = "Soft Texture Weekly Meal Pack",
            Description = "A weekly meal plan with soft texture, easy-to-chew ingredients, and mild seasoning.",
            Period = MenuPeriod.Weekly,
            Price = 92.00m,
            CategoryId = softTextureCategory.CategoryId
        }
    };

        context.Menus.AddRange(menus);
        await context.SaveChangesAsync();
      }

    }

  }
}
