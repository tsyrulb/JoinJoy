using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindMatches(int userId, string interest)
        {
            var result = await _matchService.FindMatchesAsync(userId, interest);
            return Ok(result);
        }
    }
}
