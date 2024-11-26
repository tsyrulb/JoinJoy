using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserUnavailabilityController : ControllerBase
    {
        private readonly IUserUnavailabilityService _unavailabilityService;

        public UserUnavailabilityController(IUserUnavailabilityService unavailabilityService)
        {
            _unavailabilityService = unavailabilityService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUnavailability([FromBody] UserUnavailabilityRequest request)
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var result = await _unavailabilityService.AddUnavailabilityAsync(userId, request);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }

            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpDelete("{unavailabilityId}")]
        public async Task<IActionResult> RemoveUnavailability(int unavailabilityId)
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var result = await _unavailabilityService.RemoveUnavailabilityAsync(userId, unavailabilityId);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }

            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpGet]
        public async Task<IActionResult> GetUnavailabilities()
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var unavailabilities = await _unavailabilityService.GetUnavailabilitiesAsync(userId);
                return Ok(unavailabilities);
            }

            return Unauthorized("User ID is missing or invalid in token.");
        }
    }
}
