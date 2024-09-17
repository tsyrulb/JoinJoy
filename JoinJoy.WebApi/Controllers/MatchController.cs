using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMatches(int userId)
        {
            var matches = await _matchService.GetUserMatchesAsync(userId);
            return Ok(matches);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatch(Match match)
        {
            var result = await _matchService.CreateMatchAsync(match);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("{matchId}/accept")]
        public async Task<IActionResult> AcceptMatch(int matchId)
        {
            var result = await _matchService.AcceptMatchAsync(matchId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
