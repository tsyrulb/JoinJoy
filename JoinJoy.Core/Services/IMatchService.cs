using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetUserMatchesAsync(int userId);
        Task<ServiceResult> CreateMatchAsync(Match match);
        Task<ServiceResult> AcceptMatchAsync(int matchId);
    }
}
