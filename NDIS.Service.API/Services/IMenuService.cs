using NDIS.Service.API.DTOs.Menu;

namespace NDIS.Service.API.Services
{
  public interface IMenuService
  {
    Task<IEnumerable<MenuResponseDto>> GetAllMenusAsync(string providerServiceId, string categoryId);
    Task<MenuResponseDto?> GetMenuByIdAsync(string providerServiceId, string categoryId, string menuId);
    Task<MenuResponseDto> AddMenuAsync(string providerServiceId, string categoryId, MenuCreateDto dto);
    Task<MenuResponseDto> UpdateMenuAsync(string providerServiceId, string categoryId, string menuId, MenuUpdateDto dto);
    Task DeleteMenuAsync(string providerServiceId, string categoryId, string menuId);
    Task<MenuOrderInfoResponseDto?> GetMenuOrderInfoAsync(string providerServiceId, string categoryId, string id);
  }
}
