using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;

namespace JoinJoy.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IActivityService _activityService;

        public AdminController(IUserService userService, IActivityService activityService)
        {
            _userService = userService;
            _activityService = activityService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        // TODO::Implement GetActivities method
        [HttpGet("activities")]
        public async Task<IActionResult> GetActivities()
        {
            //var result = await _activityService.GetActivitiesAsync();
            return Ok();
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete-activity")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            var result = await _activityService.DeleteActivityAsync(activityId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
