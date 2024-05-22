using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Repositories;

namespace JoinJoy.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task SubmitFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.AddAsync(feedback);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksAsync(int userId)
        {
            return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
        }
    }
}