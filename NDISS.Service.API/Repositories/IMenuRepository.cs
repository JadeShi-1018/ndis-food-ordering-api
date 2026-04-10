using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs.Menu;

namespace NDISS.Service.API.Repositories
{
  public interface IMenuRepository
  {
    Task<IEnumerable<Menu>> GetAllMenusAsync(string providerServiceId, string categoryId);
    Task<Menu?> GetMenuByIdAsync(string providerServiceId, string categoryId, string menuId);
    Task AddMenuAsync(Menu menu);
    void RemoveMenu(Menu menu);
    Task<Category?> GetCategoryAsync(string providerServiceId, string categoryId);
    Task SaveChangesAsync();
    Task<Menu?> GetMenuAsync(string providerServiceId, string categoryId, string menuId);
    Task<MenuOrderInfoResponseDto?> GetMenuOrderInfoAsync(string providerServiceId, string categoryId, string id);
  }
}
