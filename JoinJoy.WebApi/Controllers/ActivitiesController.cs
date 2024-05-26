using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Activity activity)
        {
            var result = await _activityService.CreateActivityAsync(activity);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var result = await _activityService.GetActivitiesAsync();
            return Ok(result);
        }
    }
}
