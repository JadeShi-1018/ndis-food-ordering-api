using NDISS.Service.API.Domain;
using NDISServiceAPI.DTO.Item;

namespace NDISServiceAPI.DataAcess.Repository.ItemRepository
{
    public interface IItemRepository
    {
        Task<bool> AddItem(Item request);
        Task<IEnumerable<Item>> GetAllItems();
        Task<Item> GetById(string itemId);
        Task<bool> Update(Item request);
        Task<bool> Delete(string itemId);
    }

}
