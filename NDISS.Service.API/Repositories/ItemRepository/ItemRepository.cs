using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NDISS.Service.API.Data;
using NDISS.Service.API.Domain;
using NDISServiceAPI.DTO.Item;

namespace NDISServiceAPI.DataAcess.Repository.ItemRepository
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _serdbcontext;
        private readonly IMapper _mapper;
        public ItemRepository(AppDbContext context, IMapper mapper)
        {
            _serdbcontext = context;
            _mapper = mapper;
        }

        public async Task<bool> AddItem(Item request)
        {
            await _serdbcontext.Items.AddAsync(request);
            int changes = await _serdbcontext.SaveChangesAsync();
            return changes > 0;
        }


        public async Task<IEnumerable<Item>> GetAllItems()
        {
            List<Item> items = await _serdbcontext.Items.ToListAsync();
            var dtos = _mapper.Map<List<Item>>(items);
            return dtos;
        }

        public async Task<Item> GetById(string itemId)
        {
            var item = await _serdbcontext.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
            }
            return item;
        }

        public Task<bool> Update(string itemId, Item request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Item request)
        {
            //var item = await _serdbcontext.Items.FirstOrDefaultAsync(i => i.ItemId == request.ItemId);
            //item.ItemName = request.ItemName;
            //item.ItemDescription = request.ItemDescription;
            //item.ProviderServiceId = request.ProviderServiceId;

            _serdbcontext.Items.Update(request);
            int changes = await _serdbcontext.SaveChangesAsync();
            return changes > 0;
        }


        public async Task<bool> Delete(string itemId)
        {
            var item = await _serdbcontext.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID '{itemId}' not found.");
            }

            _serdbcontext.Items.Remove(item);
            int changes = await _serdbcontext.SaveChangesAsync();
            return changes > 0;
        }


    }
}
