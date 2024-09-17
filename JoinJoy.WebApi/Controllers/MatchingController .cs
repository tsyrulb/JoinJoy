using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using JoinJoy.Infrastructure.Services;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        [HttpGet("find-matches")]
        public async Task<IActionResult> FindMatchesAsync()
        {
            var matches = await _matchingService.FindMatchesAsync();
            return Ok(matches);
        }

        // GET: api/matching/users-with-details
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _matchingService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NotFound("No users found with details.");
            }
            return Ok(users);
        }
        [HttpGet("activities")]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _matchingService.GetAllActivitiesAsync();
            if (activities == null || !activities.Any())
            {
                return NotFound("No activities found with details.");
            }
            return Ok(activities);
        }
        [HttpGet("user-activities")]
        public async Task<IActionResult> GetAllUserActivities()
        {
            var activities = await _matchingService.GetAllUserActivitiesAsync();
            if (activities == null || !activities.Any())
            {
                return NotFound("No useractivities found with details.");
            }
            return Ok(activities);
        }

        [HttpGet("subcategories")]
        public async Task<IActionResult> GetAllSubcategories()
        {
            var subcat = await _matchingService.GetAllSubcategoryAsync();
            if (subcat == null || !subcat.Any())
            {
                return NotFound("No subcat found with details.");
            }
            return Ok(subcat);
        }
        [HttpGet("user-subcategories")]
        public async Task<IActionResult> GetAllUserSubcategories()
        {
            var userSubcategories = await _matchingService.GetAllUserSubcategoryAsync();
            if (userSubcategories == null || !userSubcategories.Any())
            {
                return NotFound("No userSubcategories found with details.");
            }
            return Ok(userSubcategories);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _matchingService.GetAllCategoryAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found with details.");
            }
            return Ok(categories);
        }

    }
}
