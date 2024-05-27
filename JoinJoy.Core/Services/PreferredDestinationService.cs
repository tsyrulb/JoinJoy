using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class PreferredDestinationService : IPreferredDestinationService
    {
        private readonly IRepository<PreferredDestination> _preferredDestinationRepository;

        public PreferredDestinationService(IRepository<PreferredDestination> preferredDestinationRepository)
        {
            _preferredDestinationRepository = preferredDestinationRepository;
        }

        public async Task<IEnumerable<PreferredDestination>> GetAllPreferredDestinationsAsync()
        {
            return await _preferredDestinationRepository.GetAllAsync();
        }

        public async Task<PreferredDestination> GetPreferredDestinationByIdAsync(int id)
        {
            return await _preferredDestinationRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddPreferredDestinationAsync(PreferredDestination preferredDestination)
        {
            await _preferredDestinationRepository.AddAsync(preferredDestination);
            return new ServiceResult { Success = true, Message = "Preferred Destination added successfully" };
        }

        public async Task<ServiceResult> UpdatePreferredDestinationAsync(PreferredDestination preferredDestination)
        {
            await _preferredDestinationRepository.UpdateAsync(preferredDestination);
            return new ServiceResult { Success = true, Message = "Preferred Destination updated successfully" };
        }

        public async Task<ServiceResult> DeletePreferredDestinationAsync(int id)
        {
            var preferredDestination = await _preferredDestinationRepository.GetByIdAsync(id);
            if (preferredDestination != null)
            {
                await _preferredDestinationRepository.RemoveAsync(preferredDestination);
                return new ServiceResult { Success = true, Message = "Preferred Destination deleted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Preferred Destination not found" };
        }
    }
}
