using JoinJoy.Core.Models;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<Location> AddOrUpdateAsync(Location location);

        Task<bool> IsLocationReferencedAsync(int locationId);
    }
}