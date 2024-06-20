using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
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
            var result = await _activityService.CreateActivityAsync(activityRequest);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
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
