using Inventory.Entities;
using Inventory.Model;

namespace Inventory.Services
{
    public interface IItemService
    {
        Task<ItemDto?> GetItemAsync(int id);
        Task<Item?> AddItemAsync(ItemDto item);
        Task<IEnumerable<ItemDto>> GetItemsAsync();
        Task<IEnumerable<ItemDto>> GetItemsAsync(string? name);
        Task<bool> DeleteItemAsync(int id);
        Task<Item?> UpdateItemAsync(int id, ItemDto item);
        Task<IEnumerable<Item>> GetCategorywiseItemsAsync(int categoryId);
    }
}