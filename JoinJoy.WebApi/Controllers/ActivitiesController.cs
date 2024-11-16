using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [Authorize]  // Require authentication for all actions in this controller
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] ActivityRequest activityRequest)
        {
            // Extract user ID from JWT token if needed
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                activityRequest.CreatedById = userId; // Assuming `UserId` exists in `ActivityRequest`

                var result = await _activityService.CreateActivityAsync(activityRequest);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result.Message);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpPost("create-with-coordinates")]
        public async Task<IActionResult> CreateActivityWithCoordinates([FromBody] ActivityRequestWithCoordinates request)
        {
            if (request == null)
            {
                return BadRequest("Request body is null.");
            }

            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                request.CreatedById = userId; // Assuming `UserId` exists in `ActivityRequestWithCoordinates`

                var result = await _activityService.CreateActivityWithCoordinatesAsync(request);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpPut("{activityId}")]
        public async Task<IActionResult> UpdateActivity(int activityId, [FromBody] ActivityRequest activityRequest)
        {
            var result = await _activityService.UpdateActivityAsync(activityId, activityRequest);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            var result = await _activityService.DeleteActivityAsync(activityId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetAllActivities()
        {
            var activities = await _activityService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("{activityId}")]
        public async Task<ActionResult<Activity>> GetActivityById(int activityId)
        {
            var activity = await _activityService.GetActivityByIdAsync(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        [HttpPost("{activityId}/addUsers")]
        public async Task<IActionResult> AddUsersToActivity(int activityId, [FromBody] List<int> userIds)
        {
            var result = await _activityService.AddUsersToActivityAsync(activityId, userIds);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
