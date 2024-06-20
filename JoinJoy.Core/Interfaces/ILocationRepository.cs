using JoinJoy.Core.Models;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<bool> IsLocationReferencedAsync(int locationId);
    }
}