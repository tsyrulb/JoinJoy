// File: JoinJoy.WebApi/Controllers/FeedbackController.cs
using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(Feedback feedback)
        {
            await _feedbackService.SubmitFeedbackAsync(feedback);
            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFeedbacks(int userId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksAsync(userId);
            return Ok(feedbacks);
        }
    }
}
