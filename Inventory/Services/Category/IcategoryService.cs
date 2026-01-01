using Inventory.Entities;
using Inventory.Model;

namespace Inventory.Services
{
    public interface IcategoryService
    {
        Task<CategoryDto?> GetCategoryAsync(int id);
        Task<Category?> AddCategoryAsync(CategoryDto category);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<bool> DeleteCategoryAsync(int id);
        Task<Category?> UpdateCategoryAsync(int id, CategoryDto category);

    }
}