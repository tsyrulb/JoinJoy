using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class InterestService : IInterestService
    {
        private readonly IRepository<Interest> _interestRepository;

        public InterestService(IRepository<Interest> interestRepository)
        {
            _interestRepository = interestRepository;
        }

        public async Task<IEnumerable<Interest>> GetAllInterestsAsync()
        {
            return await _interestRepository.GetAllAsync();
        }

        public async Task<Interest> GetInterestByIdAsync(int id)
        {
            return await _interestRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddInterestAsync(Interest interest)
        {
            await _interestRepository.AddAsync(interest);
            return new ServiceResult { Success = true, Message = "Interest added successfully" };
        }

        public async Task<ServiceResult> UpdateInterestAsync(Interest interest)
        {
            await _interestRepository.UpdateAsync(interest);
            return new ServiceResult { Success = true, Message = "Interest updated successfully" };
        }

        public async Task<ServiceResult> DeleteInterestAsync(int id)
        {
            var interest = await _interestRepository.GetByIdAsync(id);
            if (interest != null)
            {
                await _interestRepository.RemoveAsync(interest);
                return new ServiceResult { Success = true, Message = "Interest deleted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Interest not found" };
        }
    }
}
