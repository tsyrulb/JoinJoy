using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<ServiceResult> SubmitFeedbackAsync(Feedback feedback)
        {
            // Validate the feedback data before adding
            if (feedback.Rating < 1 || feedback.Rating > 5)
            {
                return new ServiceResult { Success = false, Message = "Rating must be between 1 and 5" };
            }

            feedback.Timestamp = DateTime.UtcNow;

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Feedback submitted successfully" };
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackForActivityAsync(int activityId)
        {
            return await _feedbackRepository.GetFeedbackForActivityAsync(activityId);
        }

        public async Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId)
        {
            return await _feedbackRepository.GetUserFeedbackAsync(userId);
        }

        // Add the implementation for GetFeedbackAsync
        public async Task<IEnumerable<Feedback>> GetFeedbackAsync(int userId)
        {
            return await _feedbackRepository.GetUserFeedbackAsync(userId);
        }
    }
}
