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
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Promote a user to admin
        [HttpPost("promote/{userId}")]
        public async Task<IActionResult> PromoteUserToAdmin(int userId)
        {
            var result = await _adminService.PromoteUserToAdminAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Deactivate a user
        [HttpPost("deactivate/{userId}")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var result = await _adminService.DeactivateUserAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Delete an activity
        [HttpDelete("activity/{activityId}")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            var result = await _adminService.DeleteActivityAsync(activityId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Delete a feedback
        [HttpDelete("feedback/{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            var result = await _adminService.DeleteFeedbackAsync(feedbackId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // List all users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        // List all activities
        [HttpGet("activities")]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _adminService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        // List all feedbacks
        [HttpGet("feedbacks")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbacks = await _adminService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }
    }
}
