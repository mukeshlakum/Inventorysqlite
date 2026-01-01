using Inventory.Controllers;
using Inventory.Entities;
using Inventory.Model;
using Inventory.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Test
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsCreated()
        {
            // Arrange
            var userDto = new UserDto { Username = "test", Password = "pass" };
            var user = new User { Username = "test" };
            _mockAuthService.Setup(s => s.RegisterAsync(userDto)).ReturnsAsync(user);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserExists()
        {
            // Arrange
            var userDto = new UserDto { Username = "test", Password = "pass" };
            _mockAuthService.Setup(s => s.RegisterAsync(userDto)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("User already exists!", badRequest.Value);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var userDto = new UserDto { Username = "test", Password = "pass" };
            var token = "jwt-token";
            _mockAuthService.Setup(s => s.LoginAsync(userDto)).ReturnsAsync(token);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(token, okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenCredentialsAreInvalid()
        {
            // Arrange
            var userDto = new UserDto { Username = "test", Password = "wrong" };
            _mockAuthService.Setup(s => s.LoginAsync(userDto)).ReturnsAsync((string?)null);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Username / password is wrong!", badRequest.Value);
        }

        [Fact]
        public void AuthCheck_ReturnsOk()
        {
            // Act
            var result = _controller.AuthCheck();

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
