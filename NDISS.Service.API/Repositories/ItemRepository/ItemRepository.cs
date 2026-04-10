using Microsoft.EntityFrameworkCore;
using NDISS.Service.API.Data;
using NDISS.Service.API.Domain;

namespace NDISServiceAPI.DataAcess.Repository.ItemRepository
{
  public class ItemRepository : IItemRepository
  {
    private readonly AppDbContext _serdbcontext;

    public ItemRepository(AppDbContext context)
    {
      _serdbcontext = context;
    }

    public async Task<bool> AddItem(Item request)
    {
      await _serdbcontext.Items.AddAsync(request);
      int changes = await _serdbcontext.SaveChangesAsync();
      return changes > 0;
    }

    public async Task<IEnumerable<Item>> GetAllItems()
    {
      return await _serdbcontext.Items.ToListAsync();
    }

    public async Task<Item?> GetById(string itemId)
    {
      return await _serdbcontext.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
    }

    public async Task<bool> Update(Item request)
    {
      _serdbcontext.Items.Update(request);
      int changes = await _serdbcontext.SaveChangesAsync();
      return changes > 0;
    }

    public async Task<bool> Delete(string itemId)
    {
      var item = await _serdbcontext.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);

      if (item == null)
      {
        return false;
      }

      _serdbcontext.Items.Remove(item);
      int changes = await _serdbcontext.SaveChangesAsync();
      return changes > 0;
    }
  }
}