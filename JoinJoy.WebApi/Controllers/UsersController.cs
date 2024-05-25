// File: JoinJoy.WebApi/Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user != null)
                return Ok(user);
            return NotFound();
        }
    }
}
