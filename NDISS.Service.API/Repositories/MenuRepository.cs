using Microsoft.EntityFrameworkCore;
using NDISS.Service.API.Data;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs.Menu;

namespace NDISS.Service.API.Repositories
{
  public class MenuRepository : IMenuRepository
  {
    private readonly AppDbContext _context;

    public MenuRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Menu>> GetAllMenusAsync(string providerServiceId, string categoryId)
    {
      return await _context.Set<Menu>()
          .Include(m => m.Category)
          .Where(m =>
              m.CategoryId == categoryId &&
              m.Category != null &&
              m.Category.ProviderServiceId == providerServiceId)
          .AsNoTracking()
          .ToListAsync();
    }

    public async Task<Menu?> GetMenuByIdAsync(string providerServiceId, string categoryId, string menuId)
    {
      return await _context.Set<Menu>()
          .Include(m => m.Category)
          .FirstOrDefaultAsync(m =>
              m.MenuId == menuId &&
              m.CategoryId == categoryId &&
              m.Category != null &&
              m.Category.ProviderServiceId == providerServiceId);
    }

    public async Task<Category?> GetCategoryAsync(string providerServiceId, string categoryId)
    {
      return await _context.Set<Category>()
          .FirstOrDefaultAsync(c =>
              c.CategoryId == categoryId &&
              c.ProviderServiceId == providerServiceId);
    }

    public async Task AddMenuAsync(Menu menu)
    {
      await _context.Set<Menu>().AddAsync(menu);
    }

    public void RemoveMenu(Menu menu)
    {
      _context.Set<Menu>().Remove(menu);
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }

    public async Task<Menu?> GetMenuAsync(string providerServiceId, string categoryId, string menuId)
    {
      return await _context.Menus
          .Include(m => m.Category)
          .Where(m => m.MenuId == menuId
                   && m.CategoryId == categoryId
                   && m.Category.ProviderServiceId == providerServiceId)
          .FirstOrDefaultAsync();
    }

    public async Task<MenuOrderInfoResponseDto?> GetMenuOrderInfoAsync(string providerServiceId, string categoryId, string id)
    {
      var result = await (
          from menu in _context.Menus
          join category in _context.Categories
              on menu.CategoryId equals category.CategoryId
          join providerService in _context.ProviderServices
              on category.ProviderServiceId equals providerService.ProviderServiceId
          where menu.MenuId == id
                && providerService.ProviderServiceId == providerServiceId
                && category.CategoryId == categoryId
          select new MenuOrderInfoResponseDto
          {
            ProviderId = providerService.ProviderId,
            ProviderServiceId = providerService.ProviderServiceId,
            ProviderServiceName = providerService.ProviderServiceName,
            ProviderPhoneNumber = providerService.PhoneNumber,

            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,

            MenuId = menu.MenuId,
            MenuName = menu.MenuName,
            MenuDescription = menu.Description,

            PeriodName = menu.Period.ToString(),
            UnitPrice = menu.Price,
          }
      ).FirstOrDefaultAsync();

      return result;
    }
  }
}