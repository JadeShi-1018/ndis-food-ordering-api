using AutoMapper;
using NDISS.Service.API.Domain;
using NDISS.Service.API.Repositories;
using NDISServiceAPI.DataAcess.Repository.ItemRepository;
using NDISServiceAPI.DTO.Item;

namespace NDISServiceAPI.Services.ItemService
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _ItemRepository;
        private readonly IGenericRepository<ProviderService> _ProviderServiceRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository ItemRepository, IGenericRepository<ProviderService> ProviderServiceRepository, IMapper mapper)
        {
            _ItemRepository = ItemRepository;
            _ProviderServiceRepository = ProviderServiceRepository;
            _mapper = mapper;
        }


        public async Task<bool> AddItem(CreateItemRequestDto requestDto)
        {
            try
            {
                var existingProviderService = await _ProviderServiceRepository.GetByIdAsync(requestDto.ProviderServiceId);

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

                await _ItemRepository.AddItem(newItem);
                return true; 
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the item.", ex);
            }
        }




        public async Task<IEnumerable<ItemResponseDto>> GetAllItems()
        {
            var items = await _ItemRepository.GetAllItems(); 
            var dtos = _mapper.Map<List<ItemResponseDto>>(items);
            return dtos;
        }

        public async Task<ItemResponseDto> GetById(string itemId)
        {
            var item = await _ItemRepository.GetById(itemId); 
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found.");
            }
            var dto = _mapper.Map<ItemResponseDto>(item);
            return dto;
        }

        public async Task<bool> Update(string itemId, CreateItemRequestDto request)
        {
            try
            {
                var existingItem = await _ItemRepository.GetById(itemId); 
                if (existingItem == null)
                {
                    throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
                }
                var provider = await _ProviderServiceRepository.GetByIdAsync(request.ProviderServiceId);
                if (provider == null)
                {
                    throw new KeyNotFoundException($"ProviderService with ID '{request.ProviderServiceId}' does not exist.");
                }
                _mapper.Map(request, existingItem);
                await _ItemRepository.Update(existingItem); 
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the item.", ex);
            }
        }


        public async Task<bool> Delete(string itemId)
        {
            var existingItem = await _ItemRepository.GetById(itemId); 
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found.");
            }

            await _ItemRepository.Delete(itemId); 
            return true;
        }
    }
}
