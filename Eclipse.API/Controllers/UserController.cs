using Eclipse.Core.DTOs.UserDTOs;
using Eclipse.Core.Interfaces.IUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eclipse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistration userRegistration)
        {
            var result = await _userService.Register(userRegistration);
            if (result.Result)
            {
                return Ok(new { success = true, message = result.Response });
            }
            if (result.Response == "Email already exists" || result.Response == "Username already exists")
            {
                return StatusCode(409, new { message = result.Response });
            }
            return StatusCode(400, new { message = result.Response });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            HttpContext context = HttpContext;
            var result = await _userService.Login(userLogin);
            if (!result.Result)
            {
                return StatusCode(401, new { message = result.Response });
            }
            context.Response.Cookies.Append("Cooooookies", result.Token);

            return Ok(new { success = true, message = result.Response });
        }

    }
}
