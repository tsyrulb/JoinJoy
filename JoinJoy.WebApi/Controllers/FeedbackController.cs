using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;
using System.Threading.Tasks;

namespace JoinJoy.WebApi.Controllers
{
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
        public async Task<IActionResult> SubmitFeedback(FeedbackRequest feedbackRequest)
        {
            if (feedbackRequest.Rating < 1 || feedbackRequest.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var feedback = new Feedback
            {
                UserId = feedbackRequest.UserId,
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
            var result = await _feedbackService.DeleteFeedbackAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("list-for-user/{userId}")]
        public async Task<IActionResult> ListFeedbackForUser(int userId)
        {
            var feedbacks = await _feedbackService.GetUserFeedbackAsync(userId);
            return Ok(feedbacks);
        }

        [HttpGet("list-for-activity/{activityId}")]
        public async Task<IActionResult> ListFeedbackForActivity(int activityId)
        {
            var feedbacks = await _feedbackService.GetFeedbackForActivityAsync(activityId);
            return Ok(feedbacks);
        }
    }
}
