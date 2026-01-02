using Inventory.Controllers;
using Inventory.Entities;
using Inventory.Model;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Inventory.Test
{
    public class CategoryControllerTests
    {
        private readonly Mock<IcategoryService> _mockCategoryService;
        private readonly Mock<IItemService> _mockItemService;
        private readonly CategoryController _controller;
        private CategoryDto category;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<IcategoryService>();
            _mockItemService = new Mock<IItemService>();
            _controller = new CategoryController(_mockCategoryService.Object, _mockItemService.Object);
        }

        [Fact]
        public async Task Add_ReturnsOk_WhenCategoryAdded()
        {
            // Arrange
            var categorydto = new CategoryDto { Name = "Test2", Description = "Test Category2" };
            var category = new Category() { Name = "Test2", Description = "Test Category2" };
            _mockCategoryService.Setup(s => s.AddCategoryAsync(categorydto)).ReturnsAsync(category);

            // Act
            var result = await _controller.Add(categorydto);

            // Assert            
            //var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            //Assert.Equal("Category already exists!", badRequest.Value);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(category, okResult.Value);
        }

        [Fact]
        public async Task GetCategories_ReturnsOk_WithCategories()
        {
            // Arrange
            var categoriedto = new List<CategoryDto> {new CategoryDto { Name = "Test2", Description = "Test Category2" } };
            _mockCategoryService.Setup(s => s.GetCategoriesAsync()).ReturnsAsync(categoriedto);

            // Act
            var result = await _controller.GetCategories();      

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(categoriedto, okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenCategoryExists()
        {
            // Arrange
            var categorydto = new CategoryDto { Name = "Test2", Description = "Test Category2" };
            _mockCategoryService.Setup(s => s.GetCategoryAsync(1)).ReturnsAsync(categorydto);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(categorydto, okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_WhenCategoryNotFound()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.GetCategoryAsync(1)).ReturnsAsync((CategoryDto)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Category not found", badRequest.Value);
        }


        [Fact]
        public async Task Add_ReturnsBadRequest_WhenCategoryExists()
        {
            // Arrange
            var categorydto = new CategoryDto { Name = "Test", Description = "Test Category" };
            var category = new Category() {Id=1, Name = "Test2", Description = "Test Category2" };
            _mockCategoryService.Setup(s => s.AddCategoryAsync(categorydto)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.Add(categorydto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Category already exists!", badRequest.Value);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenCategoryUpdated()
        {
            // Arrange
            var categorydto = new CategoryDto { Name = "Test", Description = "Test Category" };
            var category = new Category() { Id = 1, Name = "Test2", Description = "Test Category2" };
            _mockCategoryService.Setup(s => s.UpdateCategoryAsync(1, categorydto)).ReturnsAsync(category);

            // Act
            var result = await _controller.Update(1, categorydto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(category, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenCategoryNotFound()
        {
            // Arrange
            var categorydto = new CategoryDto { Name = "Test", Description = "Test Category" };
            var category = new Category() { Id = 1, Name = "Test2", Description = "Test Category2" };
            //_mockCategoryService.Setup(s => s.UpdateCategoryAsync(1, categorydto)).ReturnsAsync((CategoryDto)null);

            // Act
            var result = await _controller.Update(1, categorydto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Category id does not exists!", badRequest.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenCategoryDeleted()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenCategoryNotFound()
        {
            // Arrange
            _mockCategoryService.Setup(s => s.DeleteCategoryAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Category id does not exists!", badRequest.Value);
        }

        [Fact]
        public async Task GetItems_ReturnsOk_WithItems()
        {
            // Arrange
            var items = new List<Item> { new Item { Name = "Test", Description = "Test Category" } };
            _mockItemService.Setup(s => s.GetCategorywiseItemsAsync(1)).ReturnsAsync(items);

            // Act
            var result = await _controller.GetItems(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(items, okResult.Value);
        }
    }
}
