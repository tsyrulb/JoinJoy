using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace JoinJoy.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            var result = await _userService.RegisterUserAsync(user);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] dynamic loginRequest)
        {
            string email = loginRequest.email;
            string password = loginRequest.password;

            var result = await _userService.LoginAsync(email, password);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User user)
        {
            if (userId != user.Id)
            {
                return BadRequest("User ID mismatch");
            }

            var result = await _userService.UpdateUserAsync(user);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("{userId}/subcategories")]
        public async Task<IActionResult> AddUserSubcategories(int userId, [FromBody] List<int> subcategoryIds)
        {
            var result = await _userService.AddUserSubcategoriesAsync(userId, subcategoryIds);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("{userId}/subcategories/{subcategoryId}")]
        public async Task<IActionResult> RemoveUserSubcategory(int userId, int subcategoryId)
        {
            var result = await _userService.RemoveUserSubcategoryAsync(userId, subcategoryId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("{userId}/preferredDestinations")]
        public async Task<IActionResult> AddUserPreferredDestinations(int userId, [FromBody] List<UserPreferredDestination> preferredDestinations)
        {
            var result = await _userService.AddUserPreferredDestinationsAsync(userId, preferredDestinations);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("{userId}/availabilities")]
        public async Task<IActionResult> AddUserAvailabilities(int userId, [FromBody] List<UserAvailability> availabilities)
        {
            var result = await _userService.AddUserAvailabilitiesAsync(userId, availabilities);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
