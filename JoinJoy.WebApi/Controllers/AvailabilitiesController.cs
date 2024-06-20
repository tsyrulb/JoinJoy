using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilitiesController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAvailabilities(int userId, [FromBody] List<AvailabilityRequest> availabilityRequests)
        {
            var result = await _availabilityService.AddOrUpdateAvailabilityAsync(userId, availabilityRequests);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAvailability(int userId, [FromBody] AvailabilityRequest availabilityRequest)
        {
            var result = await _availabilityService.CreateAvailabilityAsync(userId, availabilityRequest);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAvailability(int userId, [FromBody] AvailabilityRequest availabilityRequest)
        {
            var result = await _availabilityService.UpdateAvailabilityAsync(userId, availabilityRequest);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("{userId}/delete/{availabilityId}")]
        public async Task<IActionResult> DeleteAvailability(int userId, int availabilityId)
        {
            var result = await _availabilityService.DeleteAvailabilityAsync(userId, availabilityId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAvailabilities(int userId)
        {
            var availabilities = await _availabilityService.GetUserAvailabilitiesAsync(userId);
            return Ok(availabilities);
        }
    }
}
