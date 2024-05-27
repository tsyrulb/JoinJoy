using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IInterestService
    {
        Task<IEnumerable<Interest>> GetAllInterestsAsync();
        Task<Interest> GetInterestByIdAsync(int id);
        Task<ServiceResult> AddInterestAsync(Interest interest);
        Task<ServiceResult> UpdateInterestAsync(Interest interest);
        Task<ServiceResult> DeleteInterestAsync(int id);
    }
}
