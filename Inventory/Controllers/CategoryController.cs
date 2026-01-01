using Inventory.Entities;
using Inventory.Model;
using Inventory.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Route("api/categories")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        public IcategoryService _categoryService { get; }
        public IItemService _itemService { get; }

        public CategoryController(IcategoryService categoryservice,IItemService itemService )

        {
            _categoryService = categoryservice;
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                var catlist = await _categoryService.GetCategoriesAsync();
                return Ok(catlist);
            }            
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }

        [HttpGet("{id}")]
        public async Task< ActionResult<CategoryDto>> Get(int id)
        {
            try
            {
                var cat = await _categoryService.GetCategoryAsync(id);
                if (cat == null)
                {
                    return BadRequest("Category not found");
                }
                return Ok(cat);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }

        }

        [HttpPost()]
        public async Task< ActionResult<CategoryDto>> Add(CategoryDto category) 
        {
            try
            {
                var cat = await _categoryService.AddCategoryAsync(category);
                if (cat == null)
                {
                    return BadRequest("Category already exists!");
                }
                return Ok(cat);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }

        }
        [HttpPut("{id}")]
        public async Task< ActionResult<CategoryDto>> Update(int id, CategoryDto category)
        {
            try
            {
                var cat = await _categoryService.UpdateCategoryAsync(id, category);
                if (cat == null)
                {
                    return BadRequest("Category id does not exists!");
                }
                return Ok(cat);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }

        [HttpDelete("{id}")]
        public async Task< ActionResult> Delete(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (result == false)
                {
                    return BadRequest("Category id does not exists!");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }
        [HttpGet("/{id}/items")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems(int id)
        {
            try
            {
                var items = await _itemService.GetCategorywiseItemsAsync(id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }
    }
}
