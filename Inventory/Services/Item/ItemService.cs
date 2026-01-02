using Inventory.Data;
using Inventory.Entities;
using Inventory.Model;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Client;
using Microsoft.OpenApi.Writers;
using System.Linq;

namespace Inventory.Services
{
    public class ItemService : IItemService
    {
        public MyDbContext dbContext { get; }
        public ItemService(MyDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }        

        public async Task<Item?> AddItemAsync(ItemDto itemdto)
        {
            if (await dbContext.Items.AnyAsync(a => a.Name == itemdto.Name))
            {
                return null;
            }
            var categoryId = GetCategoryId(itemdto.Category);
            if (categoryId == 0)
                return null;

            var item = new Item();
            item.Description = itemdto.Description;
            item.Name = itemdto.Name;
            item.Price = itemdto.Price;
            item.CategoryID = categoryId;
            
            await dbContext.Items.AddAsync(item);
            await dbContext.SaveChangesAsync();
            return (item);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await dbContext.Items.FirstOrDefaultAsync(a => a.Id == id);
            if (item == null)
                return false;
            dbContext.Items.Remove(item);
            return true;
        }

        public async Task<ItemDto?> GetItemAsync(int id)
        {
            try
            {
                var item = dbContext.Items.FirstOrDefault(a => a.Id == id);
                if (item == null)
                {
                    return null;
                }
                var category = GetCategoryName(item.CategoryID);
                return new ItemDto()
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Category = category
                };
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            return await(from itm in dbContext.Items join cat in dbContext.Categories 
                        on itm.CategoryID equals cat.Id select new ItemDto
                        {
                            Name = itm.Name, 
                            Description = itm.Description,
                            Category = cat.Name,
                            Price = itm.Price
                        } ).ToListAsync<ItemDto>();            
        }

        public async Task<Item?> UpdateItemAsync(int id, ItemDto itemdto)
        {
            var item = await dbContext.Items.FirstOrDefaultAsync(a => a.Id == id);
            if (item == null)
                return null;
            var categoryId = GetCategoryId(itemdto.Category);
            if (categoryId == 0)
                return null;

            item.Description = itemdto.Description;
            item.Name = itemdto.Name;
            item.CategoryID = categoryId;
            item.Price = itemdto.Price;

            dbContext.Items.Update(item);
            await dbContext.SaveChangesAsync();
            return (item);
        }

        public async Task<IEnumerable<Item>> GetCategorywiseItemsAsync(int categoryId)
        {
            return(await dbContext.Items.Where(a=> a.CategoryID == categoryId).ToListAsync<Item>());
            //return items;
        }

        private string GetCategoryName(int categoryId)
        {
            var category = dbContext.Categories.FirstOrDefault(a => a.Id == categoryId);
            return category.Name ?? string.Empty;
        }
        private int GetCategoryId(string name)
        {
            var category = dbContext.Categories.FirstOrDefault(a => a.Name == name);
           if (category == null)
                return 0;
           return category.Id;
        }

        async Task<IEnumerable<ItemDto>> IItemService.GetItemsAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return await GetItemsAsync();

            //var collection = dbContext.Items as IQueryable<Item>;
            name = name.Trim();

            return await (from itm in dbContext.Items
                          join cat in dbContext.Categories
                        on itm.CategoryID equals cat.Id
                        where itm.Name == name
                          select new ItemDto
                          {
                              Name = itm.Name,
                              Description = itm.Description,
                              Category = cat.Name,
                              Price = itm.Price
                          }).ToListAsync<ItemDto>();
        }
    }
}
