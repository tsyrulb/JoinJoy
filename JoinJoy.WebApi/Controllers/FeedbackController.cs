using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
    [Authorize] // Require authentication for all actions
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            if (feedbackRequest.Rating < 1 || feedbackRequest.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            // Extract user ID from JWT token
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in the token.");
            }

            var feedback = new Feedback
            {
                UserId = userId, // Use the authenticated user ID
                ActivityId = feedbackRequest.ActivityId,
                TargetUserId = feedbackRequest.TargetUserId,
                Rating = feedbackRequest.Rating,
                Timestamp = DateTime.UtcNow
            };

            var result = await _feedbackService.SubmitFeedbackAsync(feedback);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // Update feedback
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackRequest feedbackRequest)
        {
            // Extract user ID from JWT token
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in the token.");
            }

            feedbackRequest.UserId = userId; // Ensure the user can only update their own feedback

            var result = await _feedbackService.UpdateFeedbackAsync(id, feedbackRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Delete feedback
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            // Extract user ID from JWT token
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in the token.");
            }

            var feedback = await _feedbackService.GetFeedbackAsync(id);
            if (feedback == null || feedback.UserId != userId)
            {
                return Unauthorized("You are not authorized to delete this feedback.");
            }

            var result = await _feedbackService.DeleteFeedbackAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("list-for-user")]
        public async Task<IActionResult> ListFeedbackForUser()
        {
            // Extract user ID from JWT token
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in the token.");
            }

            var feedbacks = await _feedbackService.GetUserFeedbackAsync(userId);
            return Ok(feedbacks);
        }

        [HttpGet("list-for-activity/{activityId}")]
        public async Task<IActionResult> ListFeedbackForActivity(int activityId)
        {
            // Extract user ID from JWT token for additional validation if needed
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in the token.");
            }

            var feedbacks = await _feedbackService.GetFeedbackForActivityAsync(activityId);
            return Ok(feedbacks);
        }
    }
}
