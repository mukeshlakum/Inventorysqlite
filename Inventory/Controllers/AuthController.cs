using Inventory.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Inventory.Services.Auth;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthService service { get; }

        public AuthController(IAuthService authService) 
        {
            service = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User?>>Register(UserDto request)
        {
            try
            {
                var user = await service.RegisterAsync(request);
                if (user is null)
                {
                    return BadRequest("User already exists!");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            try
            {
                var token = await service.LoginAsync(request);
                if (token is null)
                    return BadRequest("Username / password is wrong!");

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }

        [HttpGet("Auth-endpoint")]
        [Authorize]
        public ActionResult AuthCheck()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Something went Wrong!");
            }
        }
        

    }
}
