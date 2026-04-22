using NDIS.Service.API.Domain;

namespace NDIS.Service.API.Data
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
        var mealDeliveryType = context.ServiceTypes.First(st => st.ServiceTypeName == "Meal Delivery");
        var providerSeedItems = BuildProviderServiceSeedItems();

        var providerServices = providerSeedItems.Select(item => new ProviderService
        {
          ProviderServiceId = Guid.NewGuid().ToString(),
          ProviderServiceName = item.ProviderServiceName,
          ProviderId = item.ProviderId,
          ServiceTypeId = mealDeliveryType.ServiceTypeId,
          PhoneNumber = item.PhoneNumber,
          OpeningHours = item.OpeningHours,
          Address = item.Address,
          City = item.City,
          State = "VIC",
          Postcode = item.Postcode,
          Lat = item.Lat,
          Long = item.Long
        }).ToList();

        context.ProviderServices.AddRange(providerServices);
        await context.SaveChangesAsync();
      }

      if (!context.Categories.Any())
      {
        var providerServices = context.ProviderServices.ToList();
        var providerArchetypeMap = BuildProviderArchetypeMap();
        var categories = new List<Category>();

        foreach (var provider in providerServices)
        {
          if (!providerArchetypeMap.TryGetValue(provider.ProviderServiceName, out var archetype))
          {
            archetype = ProviderArchetype.HomeStyle;
          }

          foreach (var categoryTemplate in GetCategoryTemplatesForArchetype(archetype))
          {
            categories.Add(new Category
            {
              CategoryId = Guid.NewGuid().ToString(),
              CategoryName = categoryTemplate.Name,
              CategoryDescription = categoryTemplate.Description,
              ProviderServiceId = provider.ProviderServiceId
            });
          }
        }

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
      }

      if (!context.Menus.Any())
      {
        var categories = context.Categories.ToList();
        var menus = new List<Menu>();

        foreach (var category in categories)
        {
          foreach (var menuTemplate in GetMenusForCategory(category.CategoryName))
          {
            menus.Add(new Menu
            {
              MenuId = Guid.NewGuid().ToString(),
              MenuName = menuTemplate.Name,
              Description = menuTemplate.Description,
              Period = menuTemplate.Period,
              Price = menuTemplate.Price,
              CategoryId = category.CategoryId
            });
          }
        }

        context.Menus.AddRange(menus);
        await context.SaveChangesAsync();
      }
    }

    private static List<ProviderServiceSeedItem> BuildProviderServiceSeedItems()
    {
      var providerIds = new[]
      {
                SeedConstants.Provider1Id,
                SeedConstants.Provider2Id,
                SeedConstants.Provider3Id,
                SeedConstants.Provider4Id,
                SeedConstants.Provider5Id,
                SeedConstants.Provider6Id,
                SeedConstants.Provider7Id,
                SeedConstants.Provider8Id,
                SeedConstants.Provider9Id,
                SeedConstants.Provider10Id
            };

      var suburbs = new List<(string City, string Address, int Postcode, double Lat, double Long)>
            {
                ("Box Hill", "12 Station St", 3128, -37.8189, 145.1253),
                ("Glen Waverley", "88 Kingsway", 3150, -37.8794, 145.1647),
                ("Clayton", "21 Clayton Rd", 3168, -37.9151, 145.1296),
                ("Burwood", "45 Burwood Hwy", 3125, -37.8508, 145.1147),
                ("Doncaster", "619 Doncaster Rd", 3108, -37.7872, 145.1236),
                ("Richmond", "128 Swan St", 3121, -37.8241, 144.9988),
                ("Melbourne", "300 Collins St", 3000, -37.8153, 144.9631),
                ("Footscray", "72 Hopkins St", 3011, -37.8010, 144.9008),
                ("Carlton", "201 Lygon St", 3053, -37.7988, 144.9677),
                ("Springvale", "19 Buckingham Ave", 3171, -37.9487, 145.1522),
                ("South Yarra", "15 Toorak Rd", 3141, -37.8396, 144.9915),
                ("St Kilda", "41 Acland St", 3182, -37.8676, 144.9800),
                ("Hawthorn", "77 Glenferrie Rd", 3122, -37.8210, 145.0362),
                ("Camberwell", "510 Burke Rd", 3124, -37.8389, 145.0691),
                ("Preston", "105 High St", 3072, -37.7385, 145.0004),
                ("Coburg", "233 Sydney Rd", 3058, -37.7424, 144.9637),
                ("Sunshine", "280 Hampshire Rd", 3020, -37.7812, 144.8325),
                ("Moonee Ponds", "12 Puckle St", 3039, -37.7664, 144.9248),
                ("Brunswick", "401 Sydney Rd", 3056, -37.7678, 144.9621),
                ("Dandenong", "56 Lonsdale St", 3175, -37.9876, 145.2140)
            };

      var templates = new List<(string Prefix, ProviderArchetype Archetype)>
            {
                ("Homestyle Pantry", ProviderArchetype.HomeStyle),
                ("CarePlus Kitchen", ProviderArchetype.SeniorFriendly),
                ("GreenLife Meals", ProviderArchetype.Healthy),
                ("Healthy Bites", ProviderArchetype.Healthy),
                ("FreshCare Performance Meals", ProviderArchetype.HighProtein),
                ("Daily Nutrition Kitchen", ProviderArchetype.HighProtein),
                ("Golden Spoon Asian Kitchen", ProviderArchetype.AsianFusion),
                ("Sunrise Family Meals", ProviderArchetype.FamilyPacks),
                ("Comfort Bowl Kitchen", ProviderArchetype.SeniorFriendly),
                ("Balanced Table Meals", ProviderArchetype.HomeStyle)
            };

      var items = new List<ProviderServiceSeedItem>();
      var count = 0;

      for (int round = 0; round < 10; round++)
      {
        for (int i = 0; i < templates.Count; i++)
        {
          var suburb = suburbs[(round + i) % suburbs.Count];
          var providerId = providerIds[count % providerIds.Length];
          var template = templates[i];

          items.Add(new ProviderServiceSeedItem(
              ProviderServiceName: $"{template.Prefix} {suburb.City} #{count + 1}",
              ProviderId: providerId,
              PhoneNumber: $"0400{(count + 1).ToString("D6")}",
              Address: suburb.Address,
              City: suburb.City,
              Postcode: suburb.Postcode,
              Lat: suburb.Lat + (count * 0.0001),
              Long: suburb.Long + (count * 0.0001),
              OpeningHours: GetOpeningHours(template.Archetype),
              Archetype: template.Archetype
          ));

          count++;
        }
      }

      return items;
    }

    private static Dictionary<string, ProviderArchetype> BuildProviderArchetypeMap()
    {
      return BuildProviderServiceSeedItems()
          .ToDictionary(x => x.ProviderServiceName, x => x.Archetype);
    }

    private static string GetOpeningHours(ProviderArchetype archetype)
    {
      return archetype switch
      {
        ProviderArchetype.HomeStyle => "Mon-Sun 9:00-19:00",
        ProviderArchetype.SeniorFriendly => "Mon-Sun 8:00-18:00",
        ProviderArchetype.Healthy => "Mon-Sat 8:30-18:30",
        ProviderArchetype.HighProtein => "Mon-Sun 7:00-20:00",
        ProviderArchetype.AsianFusion => "Mon-Sun 11:00-21:00",
        ProviderArchetype.FamilyPacks => "Mon-Sun 9:00-20:00",
        _ => "Mon-Sun 9:00-18:00"
      };
    }

    private static List<CategoryTemplate> GetCategoryTemplatesForArchetype(ProviderArchetype archetype)
    {
      return archetype switch
      {
        ProviderArchetype.HomeStyle => new List<CategoryTemplate>
                {
                    new("Home Style Meals", "Comforting home-style meals designed for daily support needs."),
                    new("Warm Meals", "Warm prepared meals ideal for lunch and dinner."),
                    new("Balanced Daily Meals", "Simple, balanced meals for everyday nutrition.")
                },

        ProviderArchetype.Healthy => new List<CategoryTemplate>
                {
                    new("Vegetarian", "Balanced vegetarian meals with fresh ingredients."),
                    new("Low Sugar", "Meals designed for participants who prefer lower sugar options."),
                    new("Weekly Meal Packs", "Convenient weekly meal packs with balanced nutrition.")
                },

        ProviderArchetype.HighProtein => new List<CategoryTemplate>
                {
                    new("High Protein", "Protein-rich meals to support recovery and daily energy."),
                    new("Recovery Meals", "Meals focused on strength, recovery, and nutrition."),
                    new("Balanced Daily Meals", "Well-balanced meals with protein, vegetables, and grains.")
                },

        ProviderArchetype.SeniorFriendly => new List<CategoryTemplate>
                {
                    new("Soft Texture", "Soft and easy-to-eat meals suitable for specific dietary needs."),
                    new("Easy to Digest", "Gentle meals designed for participants who prefer lighter options."),
                    new("Soup and Congee", "Warm soup and congee options with mild seasoning.")
                },

        ProviderArchetype.AsianFusion => new List<CategoryTemplate>
                {
                    new("Asian Rice Bowls", "Rice-based meals inspired by familiar Asian flavours."),
                    new("Noodle Meals", "Prepared noodle meals with balanced ingredients."),
                    new("Weekly Meal Packs", "Convenient weekly meal packs with Asian-style options.")
                },

        ProviderArchetype.FamilyPacks => new List<CategoryTemplate>
                {
                    new("Family Meal Packs", "Larger meal portions designed for shared support needs."),
                    new("Home Style Meals", "Comfort meals suitable for regular daily support."),
                    new("Weekly Meal Packs", "Prepared meal packs for weekly planning and convenience.")
                },

        _ => new List<CategoryTemplate>
                {
                    new("Balanced Daily Meals", "Well-balanced daily prepared meals."),
                    new("Home Style Meals", "Comforting prepared home-style meals."),
                    new("Weekly Meal Packs", "Convenient meal packs for weekly planning.")
                }
      };
    }

    private static List<MenuTemplate> GetMenusForCategory(string categoryName)
    {
      return categoryName switch
      {
        "Home Style Meals" => new List<MenuTemplate>
                {
                    new("Braised Chicken with Rice", "Tender braised chicken with rice and seasonal vegetables.", MenuPeriod.Daily, 16.90m),
                    new("Tomato Egg Rice Bowl", "A simple and comforting tomato egg rice bowl with light seasoning.", MenuPeriod.Daily, 14.20m),
                    new("Beef and Potato Stew Set", "Slow-cooked beef with potato and a warm side dish.", MenuPeriod.Daily, 17.80m),
                    new("Homestyle Weekly Meal Pack", "A weekly pack of warm, home-style meals designed for convenience.", MenuPeriod.Weekly, 89.00m)
                },

        "Warm Meals" => new List<MenuTemplate>
                {
                    new("Chicken Soup Set", "A warm chicken soup served with a balanced side.", MenuPeriod.Daily, 14.80m),
                    new("Warm Curry Rice Meal", "Mild curry rice with vegetables and protein.", MenuPeriod.Daily, 16.20m),
                    new("Lamb Stew Dinner", "A hearty lamb stew with soft vegetables.", MenuPeriod.Daily, 18.50m),
                    new("Warm Dinner Weekly Pack", "A weekly pack of warm lunch and dinner meals.", MenuPeriod.Weekly, 94.00m)
                },

        "Balanced Daily Meals" => new List<MenuTemplate>
                {
                    new("Balanced Chicken Meal", "Chicken, vegetables, and grains in a balanced daily portion.", MenuPeriod.Daily, 16.50m),
                    new("Turkey Rice Plate", "Turkey with rice and vegetables for everyday nutrition.", MenuPeriod.Daily, 17.00m),
                    new("Daily Wellness Bowl", "A balanced bowl with protein, greens, and whole grains.", MenuPeriod.Daily, 16.80m),
                    new("Balanced Weekly Meal Pack", "A weekly meal pack for simple daily nutrition.", MenuPeriod.Weekly, 90.00m)
                },

        "Vegetarian" => new List<MenuTemplate>
                {
                    new("Vegetable Tofu Bowl", "High-protein tofu with seasonal vegetables and rice.", MenuPeriod.Daily, 15.90m),
                    new("Mushroom Pasta Pack", "Light pasta with mushroom and herbs.", MenuPeriod.Daily, 15.20m),
                    new("Pumpkin and Lentil Tray", "A balanced vegetarian tray with pumpkin and lentils.", MenuPeriod.Daily, 14.90m),
                    new("Vegetarian Weekly Pack", "A weekly vegetarian meal plan with varied options.", MenuPeriod.Weekly, 84.00m)
                },

        "Low Sugar" => new List<MenuTemplate>
                {
                    new("Steamed Fish and Greens", "Lightly seasoned fish with greens and balanced nutrition.", MenuPeriod.Daily, 18.20m),
                    new("Low Sugar Chicken Set", "Chicken with vegetables and controlled sugar content.", MenuPeriod.Daily, 16.80m),
                    new("Low Sugar Congee Set", "A warm and mild congee option with low sugar ingredients.", MenuPeriod.Daily, 13.80m),
                    new("Low Sugar Weekly Meal Pack", "A weekly meal pack designed for lower sugar preferences.", MenuPeriod.Weekly, 89.00m)
                },

        "Weekly Meal Packs" => new List<MenuTemplate>
                {
                    new("Balanced Weekly Pack", "A weekly set of balanced daily meals.", MenuPeriod.Weekly, 86.00m),
                    new("Family Support Meal Pack", "A convenient weekly meal pack for shared support needs.", MenuPeriod.Weekly, 96.00m),
                    new("Nutrition Weekly Combo", "A varied weekly meal combo with protein and vegetables.", MenuPeriod.Weekly, 91.00m),
                    new("Wellness Weekly Plan", "A weekly meal plan supporting simple and consistent nutrition.", MenuPeriod.Weekly, 88.00m)
                },

        "High Protein" => new List<MenuTemplate>
                {
                    new("Grilled Chicken Bowl", "Lean grilled chicken with rice and vegetables.", MenuPeriod.Daily, 17.20m),
                    new("Beef Recovery Set", "High-protein beef meal designed for strength and recovery.", MenuPeriod.Daily, 18.40m),
                    new("Salmon Protein Plate", "Salmon with grains and greens for balanced protein intake.", MenuPeriod.Daily, 19.50m),
                    new("High Protein Weekly Pack", "A weekly meal pack focused on protein and recovery.", MenuPeriod.Weekly, 98.00m)
                },

        "Recovery Meals" => new List<MenuTemplate>
                {
                    new("Recovery Chicken Tray", "A balanced tray with lean chicken and recovery-focused nutrition.", MenuPeriod.Daily, 17.60m),
                    new("Protein Rice Recovery Bowl", "Rice bowl with high protein and vegetables.", MenuPeriod.Daily, 16.90m),
                    new("Beef Strength Meal", "A hearty beef meal to support energy and recovery.", MenuPeriod.Daily, 18.90m),
                    new("Recovery Weekly Plan", "A weekly meal plan for energy, protein, and convenience.", MenuPeriod.Weekly, 99.00m)
                },

        "Soft Texture" => new List<MenuTemplate>
                {
                    new("Soft Chicken Congee", "Smooth rice congee with shredded chicken and mild flavour.", MenuPeriod.Daily, 13.50m),
                    new("Pumpkin Mash with Fish", "Soft pumpkin mash paired with tender fish.", MenuPeriod.Daily, 15.60m),
                    new("Minced Beef Soft Bowl", "A soft bowl with minced beef, vegetables, and gentle seasoning.", MenuPeriod.Daily, 15.90m),
                    new("Soft Texture Weekly Pack", "A weekly meal plan featuring easy-to-eat soft texture meals.", MenuPeriod.Weekly, 92.00m)
                },

        "Easy to Digest" => new List<MenuTemplate>
                {
                    new("Light Chicken Soup", "A gentle chicken soup with soft ingredients.", MenuPeriod.Daily, 13.90m),
                    new("Steamed Fish Rice Set", "Light fish with soft rice and mild seasoning.", MenuPeriod.Daily, 15.40m),
                    new("Gentle Vegetable Bowl", "A mild vegetable bowl designed for easier digestion.", MenuPeriod.Daily, 14.50m),
                    new("Easy Digest Weekly Pack", "A weekly meal plan with light and gentle meal options.", MenuPeriod.Weekly, 87.00m)
                },

        "Soup and Congee" => new List<MenuTemplate>
                {
                    new("Chicken Congee Bowl", "A warm bowl of chicken congee with soft texture.", MenuPeriod.Daily, 12.90m),
                    new("Pumpkin Soup Set", "Smooth pumpkin soup with a light side.", MenuPeriod.Daily, 13.20m),
                    new("Fish Congee Meal", "Soft fish congee with mild seasoning.", MenuPeriod.Daily, 14.10m),
                    new("Soup and Congee Weekly Pack", "A weekly set of soups and congee meals.", MenuPeriod.Weekly, 82.00m)
                },

        "Asian Rice Bowls" => new List<MenuTemplate>
                {
                    new("Teriyaki Chicken Rice Bowl", "A rice bowl with teriyaki chicken and vegetables.", MenuPeriod.Daily, 16.80m),
                    new("Braised Beef Rice Bowl", "Slow-braised beef with rice and balanced sides.", MenuPeriod.Daily, 17.90m),
                    new("Soy Ginger Fish Bowl", "Fish rice bowl with soy ginger flavour and greens.", MenuPeriod.Daily, 17.50m),
                    new("Asian Rice Bowl Weekly Pack", "A weekly pack of Asian-style rice bowl meals.", MenuPeriod.Weekly, 93.00m)
                },

        "Noodle Meals" => new List<MenuTemplate>
                {
                    new("Chicken Noodle Meal", "Prepared chicken noodle meal with vegetables.", MenuPeriod.Daily, 15.70m),
                    new("Beef Udon Bowl", "Warm udon with beef and mild broth.", MenuPeriod.Daily, 16.90m),
                    new("Vegetable Noodle Soup", "A light noodle soup with vegetables and gentle seasoning.", MenuPeriod.Daily, 14.90m),
                    new("Noodle Variety Weekly Pack", "A weekly pack of noodle-based prepared meals.", MenuPeriod.Weekly, 88.00m)
                },

        "Family Meal Packs" => new List<MenuTemplate>
                {
                    new("Family Chicken Dinner Pack", "A larger chicken dinner pack suitable for shared meals.", MenuPeriod.Weekly, 105.00m),
                    new("Family Beef and Veg Pack", "Prepared beef and vegetable meal pack in larger portions.", MenuPeriod.Weekly, 110.00m),
                    new("Family Pasta Support Pack", "A weekly pasta pack designed for easy family-style support meals.", MenuPeriod.Weekly, 102.00m),
                    new("Shared Weekly Nutrition Pack", "A larger weekly meal plan for shared support needs.", MenuPeriod.Weekly, 108.00m)
                },

        _ => new List<MenuTemplate>
                {
                    new("Daily Support Meal", "A simple daily support meal with balanced nutrition.", MenuPeriod.Daily, 15.00m),
                    new("Prepared Nutrition Bowl", "Prepared meal with vegetables and protein.", MenuPeriod.Daily, 16.00m),
                    new("Care Meal Set", "A ready-to-eat meal set designed for support needs.", MenuPeriod.Daily, 16.50m),
                    new("Support Weekly Pack", "A weekly meal pack with reliable prepared options.", MenuPeriod.Weekly, 88.00m)
                }
      };
    }

    private record ProviderServiceSeedItem(
        string ProviderServiceName,
        string ProviderId,
        string PhoneNumber,
        string Address,
        string City,
        int Postcode,
        double Lat,
        double Long,
        string OpeningHours,
        ProviderArchetype Archetype
    );

    private record CategoryTemplate(string Name, string Description);

    private record MenuTemplate(string Name, string Description, MenuPeriod Period, decimal Price);

    private enum ProviderArchetype
    {
      HomeStyle,
      Healthy,
      HighProtein,
      SeniorFriendly,
      AsianFusion,
      FamilyPacks
    }
  }
}