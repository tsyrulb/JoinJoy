using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> FindMatchesAsync(int userId, string interest);
    }
}
