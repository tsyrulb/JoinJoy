// File: JoinJoy.WebApi/Controllers/MatchesController.cs
using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMatches(int userId, [FromQuery] string interest)
        {
            var matches = await _matchService.FindMatchesAsync(userId, interest);
            return Ok(matches);
        }
    }
}
