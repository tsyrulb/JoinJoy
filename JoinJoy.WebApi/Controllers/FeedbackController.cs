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
        public async Task<IActionResult> SubmitFeedback(Feedback feedback)
        {
            var result = await _feedbackService.SubmitFeedbackAsync(feedback);
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
