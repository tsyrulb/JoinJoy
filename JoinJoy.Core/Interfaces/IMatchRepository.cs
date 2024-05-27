using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        Task<IEnumerable<Match>> GetMatchesForUserAsync(int userId);
    }
}
