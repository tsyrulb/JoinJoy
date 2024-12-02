using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using JoinJoy.Infrastructure.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using JoinJoy.Core.Models;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        // GET: api/matching/users-with-details
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _matchingService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NotFound("No users found with details.");
            }
            return Ok(users);
        }
        [HttpGet("activities")]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _matchingService.GetAllActivitiesAsync();
            if (activities == null || !activities.Any())
            {
                return NotFound("No activities found with details.");
            }
            return Ok(activities);
        }
        [HttpGet("user-activities")]
        public async Task<IActionResult> GetAllUserActivities()
        {
            var activities = await _matchingService.GetAllUserActivitiesAsync();
            if (activities == null || !activities.Any())
            {
                return NotFound("No useractivities found with details.");
            }
            return Ok(activities);
        }

        [HttpGet("subcategories")]
        public async Task<IActionResult> GetAllSubcategories()
        {
            var subcat = await _matchingService.GetAllSubcategoryAsync();
            if (subcat == null || !subcat.Any())
            {
                return NotFound("No subcat found with details.");
            }
            return Ok(subcat);
        }
        [HttpGet("user-subcategories")]
        public async Task<IActionResult> GetAllUserSubcategories()
        {
            var userSubcategories = await _matchingService.GetAllUserSubcategoryAsync();
            if (userSubcategories == null || !userSubcategories.Any())
            {
                return NotFound("No userSubcategories found with details.");
            }
            return Ok(userSubcategories);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _matchingService.GetAllCategoryAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found with details.");
            }
            return Ok(categories);
        }
        [HttpGet("recommend-users")]
        public async Task<IActionResult> GetRecommendedUsersForActivity([FromQuery] int activityId, [FromQuery] int topN = 20)
        {
            try
            {
                var recommendations = await _matchingService.GetRecommendedUsersForActivityAsync(activityId, topN);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("recommend-activities")]
        public async Task<IActionResult> GetRecommendedActivitiesForUser([FromQuery] int userId, [FromQuery] int topN = 20)
        {
            try
            {
                var recommendations = await _matchingService.GetRecommendedActivitiesForUserAsync(userId, topN);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("request-approval")]
        public async Task<IActionResult> RequestApproval([FromBody] RequestApprovalRequest request)
        {
            Console.WriteLine($"Received payload: ActivityId={request.ActivityId}");
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int requesterId))
            {
                var result = await _matchingService.RequestApprovalAsync(requesterId, request.ActivityId);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpPost("send-invitations")]
        public async Task<IActionResult> SendInvitations([FromBody] InvitationRequest request)
        {
            Console.WriteLine($"Received payload: ActivityId={request.ActivityId}, ReceiverIds={string.Join(", ", request.ReceiverIds)}");
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int senderId))
            {
                var result = await _matchingService.SendInvitationsAsync(senderId, request.ActivityId, request.ReceiverIds);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpPost("accept-invitation/{matchId}")]
        public async Task<IActionResult> AcceptInvitation(int matchId)
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var result = await _matchingService.AcceptInvitationAsync(matchId, userId);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpDelete("cancel-invitation/{matchId}")]
        public async Task<IActionResult> CancelInvitation(int matchId)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in token.");
            }

            var result = await _matchingService.CancelInvitationAsync(matchId, userId);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // GET: api/matching/matches
        [HttpGet("matches")]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _matchingService.GetAllMatchesAsync();
            if (!matches.Any())
            {
                return NotFound("No matches found.");
            }
            return Ok(matches);
        }

        [HttpGet("user-matches")]
        public async Task<IActionResult> GetMatchesByUserId()
        {
            try
            {
                // Extract user ID from the token
                if (int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out int userId))
                {
                    var matches = await _matchingService.GetMatchesByUserIdAsync(userId);
                    if (matches == null || !matches.Any())
                    {
                        return NotFound($"No matches found for user with ID {userId}.");
                    }
                    return Ok(matches);
                }
                return Unauthorized("User ID is missing or invalid in the token.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving matches: {ex.Message}");
            }
        }

        // GET: api/matching/matches/{id}
        [HttpGet("matches/{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _matchingService.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound($"Match with ID {id} not found.");
            }
            return Ok(match);
        }

        // POST: api/matching/matches
        [HttpPost("matches")]
        public async Task<IActionResult> CreateMatch([FromBody] Match match)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in token.");
            }

            match.UserId1 = userId; // Set the first user as the currently logged-in user
            match.MatchDate = DateTime.UtcNow;

            var result = await _matchingService.CreateMatchAsync(match);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // PUT: api/matching/matches/{id}
        [HttpPut("matches/{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] Match updatedMatch)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in token.");
            }

            var match = await _matchingService.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound("Match not found.");
            }

            if (match.UserId1 != userId && match.User2Id != userId)
            {
                return Forbid("You are not authorized to update this match.");
            }

            updatedMatch.Id = id; // Ensure the ID matches the one being updated
            var result = await _matchingService.UpdateMatchAsync(id, updatedMatch);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // DELETE: api/matching/matches/{id}
        [HttpDelete("matches/{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in token.");
            }

            var match = await _matchingService.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound("Match not found.");
            }

            if (match.UserId1 != userId && match.User2Id != userId)
            {
                return Forbid("You are not authorized to delete this match.");
            }

            var result = await _matchingService.DeleteMatchAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        public class InvitationRequest
        {
            public int ActivityId { get; set; } // ID of the activity
            public List<int> ReceiverIds { get; set; } // List of user IDs to send invitations to
        }
        public class RequestApprovalRequest
        {
            public int ActivityId { get; set; }
        }

    }
}
