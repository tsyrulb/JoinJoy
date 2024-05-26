using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Core.Models;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.Core.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<Feedback> _feedbackRepository;

        public FeedbackService(IRepository<Feedback> feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<ServiceResult> SubmitFeedbackAsync(Feedback feedback)
        {
            // Implement submit feedback logic here
            return new ServiceResult { Success = true, Message = "Feedback submitted successfully" };
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackAsync(int userId)
        {
            return await _feedbackRepository.GetAllAsync();
        }
    }
}
