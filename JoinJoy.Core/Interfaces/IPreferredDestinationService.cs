using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IPreferredDestinationService
    {
        Task<IEnumerable<PreferredDestination>> GetAllPreferredDestinationsAsync();
        Task<PreferredDestination> GetPreferredDestinationByIdAsync(int id);
        Task<ServiceResult> AddPreferredDestinationAsync(PreferredDestination preferredDestination);
        Task<ServiceResult> UpdatePreferredDestinationAsync(PreferredDestination preferredDestination);
        Task<ServiceResult> DeletePreferredDestinationAsync(int id);
    }
}
