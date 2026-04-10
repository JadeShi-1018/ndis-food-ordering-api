using AutoMapper;
using NDISS.Service.API.Domain;
using NDISS.Service.API.Repositories;
using NDISServiceAPI.DataAcess.Repository.ItemRepository;
using NDISServiceAPI.DTO.Item;

namespace NDISServiceAPI.Services.ItemService
{
  public class ItemService : IItemService
  {
    private readonly IItemRepository _itemRepository;
    private readonly IGenericRepository<ProviderService> _providerServiceRepository;
    private readonly IMapper _mapper;

    public ItemService(
        IItemRepository itemRepository,
        IGenericRepository<ProviderService> providerServiceRepository,
        IMapper mapper)
    {
      _itemRepository = itemRepository;
      _providerServiceRepository = providerServiceRepository;
      _mapper = mapper;
    }

    public async Task<bool> AddItem(CreateItemRequestDto requestDto)
    {
      var existingProviderService = await _providerServiceRepository.GetByIdAsync(requestDto.ProviderServiceId);

      if (existingProviderService == null)
      {
        throw new KeyNotFoundException($"ProviderServiceId '{requestDto.ProviderServiceId}' does not exist.");
      }

      var newItem = new Item
      {
        ItemId = Guid.NewGuid().ToString(),
        ItemName = requestDto.ItemName,
        ProviderServiceId = requestDto.ProviderServiceId,
        ItemDescription = requestDto.ItemDescription,
      };

      return await _itemRepository.AddItem(newItem);
    }

    public async Task<IEnumerable<ItemResponseDto>> GetAllItems()
    {
      var items = await _itemRepository.GetAllItems();
      return _mapper.Map<List<ItemResponseDto>>(items);
    }

    public async Task<ItemResponseDto> GetById(string itemId)
    {
      var item = await _itemRepository.GetById(itemId);

      if (item == null)
      {
        throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
      }

      return _mapper.Map<ItemResponseDto>(item);
    }

    public async Task<bool> Update(string itemId, CreateItemRequestDto request)
    {
      var existingItem = await _itemRepository.GetById(itemId);

      if (existingItem == null)
      {
        throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
      }

      var provider = await _providerServiceRepository.GetByIdAsync(request.ProviderServiceId);

      if (provider == null)
      {
        throw new KeyNotFoundException($"ProviderService with ID '{request.ProviderServiceId}' does not exist.");
      }

      _mapper.Map(request, existingItem);

      return await _itemRepository.Update(existingItem);
    }

    public async Task<bool> Delete(string itemId)
    {
      var deleted = await _itemRepository.Delete(itemId);

      if (!deleted)
      {
        throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
      }

      return true;
    }
  }
}