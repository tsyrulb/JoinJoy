using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using Microsoft.AspNetCore.Mvc;
using DayOfWeek = JoinJoy.Core.Models.DayOfWeek;

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
        public async Task<IActionResult> RegisterUser(string name, string email, string password)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
            };
            var result = await _userService.RegisterUserAsync(user);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _userService.LoginAsync(email, password);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPut("{userId}/details")]
        public async Task<IActionResult> UpdateUserDetails(int userId, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var result = await _userService.UpdateUserDetailsAsync(userId,
                updateUserRequest.Name,
                updateUserRequest.Email,
                updateUserRequest.Password,
                updateUserRequest.ProfilePhoto,
                updateUserRequest.DateOfBirth,
                updateUserRequest.Address);
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
        public async Task<IActionResult> AddUserSubcategories(int userId, [FromBody] List<UserSubcategoryDto> subcategoryIds)
        {
            if (subcategoryIds == null || subcategoryIds.Count == 0)
            {
                return BadRequest("Subcategory IDs and weights cannot be null or empty.");
            }
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

        [HttpGet("{userId}/subcategories")]
        public async Task<ActionResult<UserSubcategory>> GeteUserSubcategory(int userId)
        {
            var result = await _userService.GetSubcategoriesByUserIdAsync(userId);
            if (result != null)
            {
                return BadRequest();
            }
            return Ok(result);
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
        
        [HttpPost("set-availability")]
        public async Task<IActionResult> SetAvailability(int userId, DayOfWeek unavailableDay, TimeSpan unavailableStartTime, TimeSpan unavailableEndTime)
        {
            var result = await _userService.SetUserAvailabilityAsync(userId, unavailableDay, unavailableStartTime, unavailableEndTime);

            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }


        [HttpPut("{userId}/distance")]
        public async Task<IActionResult> UpdateUserDistanceWillingToTravel(int userId, [FromBody] double distance)
        {
            var result = await _userService.UpdateUserDistanceWillingToTravelAsync(userId, distance);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
        
        [HttpGet("{userId}/availability")]
        public async Task<IActionResult> CheckUserAvailability(int userId)
        {
            var currentTime = DateTime.UtcNow;

            var isAvailable = await _userService.IsUserAvailableAsync(userId, currentTime);
            if (isAvailable)
            {
                return Ok(new { available = true, message = "User is available" });
            }
            else
            {
                return Ok(new { available = false, message = "User is not available" });
            }
        }
    }
}
