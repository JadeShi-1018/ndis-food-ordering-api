using NDISServiceAPI.DTO.Item;

namespace NDISServiceAPI.Services.ItemService
{
    public interface IItemService
    {
        Task<bool> AddItem(CreateItemRequestDto request);
        Task<IEnumerable<ItemResponseDto>> GetAllItems();
        Task<ItemResponseDto> GetById(string itemId);
        Task<bool> Update(string itemId, CreateItemRequestDto request);
        Task<bool> Delete(string itemId);
    }
}
