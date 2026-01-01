using Inventory.Entities;
using Inventory.Model;

namespace Inventory.Services.Auth
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(UserDto request);
        Task<User?> RegisterAsync(UserDto request);
    }
}