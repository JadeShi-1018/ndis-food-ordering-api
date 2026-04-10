using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NDIS.Service.API.Domain;
using NDIS.Service.API.DTOs.Menu;
using NDIS.Service.API.Repositories;

namespace NDIS.Service.API.Services
{
  public class MenuService : IMenuService
  {
    private readonly IMenuRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<MenuService> _logger;

    public MenuService(
        IMenuRepository repository,
        IMapper mapper,
        ILogger<MenuService> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<IEnumerable<MenuResponseDto>> GetAllMenusAsync(string providerServiceId, string categoryId)
    {
      try
      {
        var category = await _repository.GetCategoryAsync(providerServiceId, categoryId);

        if (category == null)
          throw new KeyNotFoundException("Category not found under the specified ProviderService.");

        var menus = await _repository.GetAllMenusAsync(providerServiceId, categoryId);

        return _mapper.Map<IEnumerable<MenuResponseDto>>(menus);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex,
            "Error occurred while retrieving menus for ProviderServiceId {ProviderServiceId}, CategoryId {CategoryId}",
            providerServiceId, categoryId);
        throw;
      }
    }

    public async Task<MenuResponseDto?> GetMenuByIdAsync(string providerServiceId, string categoryId, string menuId)
    {
      try
      {
        var menu = await _repository.GetMenuByIdAsync(providerServiceId, categoryId, menuId);

        if (menu == null)
          return null;

        return _mapper.Map<MenuResponseDto>(menu);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex,
            "Error occurred while retrieving MenuId {MenuId} for ProviderServiceId {ProviderServiceId}, CategoryId {CategoryId}",
            menuId, providerServiceId, categoryId);
        throw;
      }
    }

    public async Task<MenuResponseDto> AddMenuAsync(string providerServiceId, string categoryId, MenuCreateDto dto)
    {
      try
      {
        var category = await _repository.GetCategoryAsync(providerServiceId, categoryId);

        if (category == null)
          throw new KeyNotFoundException("Category not found under the specified ProviderService.");

        var menu = _mapper.Map<Menu>(dto);
        menu.MenuId = Guid.NewGuid().ToString();
        menu.CategoryId = categoryId;

        await _repository.AddMenuAsync(menu);
        await _repository.SaveChangesAsync();

        return _mapper.Map<MenuResponseDto>(menu);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex,
            "Error occurred while creating menu for ProviderServiceId {ProviderServiceId}, CategoryId {CategoryId}",
            providerServiceId, categoryId);
        throw;
      }
    }

    public async Task<MenuResponseDto> UpdateMenuAsync(string providerServiceId, string categoryId, string menuId, MenuUpdateDto dto)
    {
      try
      {
        var category = await _repository.GetCategoryAsync(providerServiceId, categoryId);

        if (category == null)
          throw new KeyNotFoundException("Category not found under the specified ProviderService.");

       
          var existingMenu = await _repository.GetMenuAsync(providerServiceId, categoryId, menuId);

          if (existingMenu == null)
            throw new KeyNotFoundException("Menu not found under the specified Category.");

          _mapper.Map(dto, existingMenu);

          existingMenu.CategoryId = categoryId;

          await _repository.SaveChangesAsync();

          return _mapper.Map<MenuResponseDto>(existingMenu);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex,
              "Error occurred while updating menu for ProviderServiceId {ProviderServiceId}, CategoryId {CategoryId}, MenuId {MenuId}",
              providerServiceId, categoryId, menuId);
          throw;
        }
      }

    

    public async Task DeleteMenuAsync(string providerServiceId, string categoryId, string menuId)
    {
      try
      {
        var menu = await _repository.GetMenuByIdAsync(providerServiceId, categoryId, menuId);

        if (menu == null)
          throw new KeyNotFoundException("Menu not found under the specified Category and ProviderService.");

        _repository.RemoveMenu(menu);
        await _repository.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex,
            "Error occurred while deleting MenuId {MenuId} for ProviderServiceId {ProviderServiceId}, CategoryId {CategoryId}",
            menuId, providerServiceId, categoryId);
        throw;
      }
    }

    public async Task<MenuOrderInfoResponseDto?> GetMenuOrderInfoAsync(string providerServiceId, string categoryId, string id)
    {
      var menuOrderInfo = await _repository.GetMenuOrderInfoAsync(providerServiceId, categoryId, id);

      if (menuOrderInfo == null)
      {
        return null;
      }

      // for future bussiness logic

      return menuOrderInfo;
    }

  }
}