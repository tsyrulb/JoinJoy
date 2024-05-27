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
            await _feedbackRepository.AddAsync(feedback);
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

        public Task<IEnumerable<Feedback>> GetFeedbackAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
