using Inventory.Model;
using Inventory.Entities;
using Inventory.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class categoryService : IcategoryService
    {
        private readonly MyDbContext dbContext;

        public categoryService(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return await (from c in  dbContext.Categories select 
                new CategoryDto {Name = c.Name, Description = c.Description }).ToListAsync<CategoryDto>();

           //List<CategoryDto> data = dbContext.Categories.Select (a=> new CategoryDto { a.Name,a.Description} ).ToList<CategoryDto>();
      
        }
        
      
        public async Task<CategoryDto?> GetCategoryAsync(int id)
    {
            var cat = await dbContext.Categories.FirstOrDefaultAsync(a => a.Id == id);
            if (cat == null)
            {
                return null;
            }
            return new CategoryDto() { Name = cat.Name,Description= cat.Description };
        }

        public async Task<Category?> AddCategoryAsync(CategoryDto category)
        {
            //var cat1 = await dbContext.Categories.FirstOrDefaultAsync(a => a.Name == category.Name);
            if (await dbContext.Categories.AnyAsync(a => a.Name == category.Name))
            {
                return null;
            }
            var cat = new Category();
            cat.Name = category.Name;
            cat.Description = category.Description;
            await dbContext.Categories.AddAsync(cat);
            await dbContext.SaveChangesAsync();
            return (cat);
        }

        public async Task<Category?> UpdateCategoryAsync(int id, CategoryDto category)
        {
            var cat =await dbContext.Categories.FirstOrDefaultAsync(a => a.Id == id);
            if (cat == null)
                return null;
            cat.Description = category.Description;
            dbContext.Categories.Update(cat);
            await dbContext.SaveChangesAsync();
            return (cat);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var cat = await dbContext.Categories.FirstOrDefaultAsync(a => a.Id == id);
            if (cat == null)
                return false;
            dbContext.Categories.Remove(cat);
            return true;
        }

       
    }
}
