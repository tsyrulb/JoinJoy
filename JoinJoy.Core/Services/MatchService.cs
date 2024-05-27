using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> GetMatchesForUserAsync(int userId)
        {
            return await _matchRepository.GetMatchesForUserAsync(userId);
        }

        public async Task<ServiceResult> AcceptMatchAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match != null)
            {
                match.IsAccepted = true;
                await _matchRepository.UpdateAsync(match);
                return new ServiceResult { Success = true, Message = "Match accepted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Match not found" };
        }

        public async Task<ServiceResult> DeclineMatchAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match != null)
            {
                match.IsAccepted = false;
                await _matchRepository.UpdateAsync(match);
                return new ServiceResult { Success = true, Message = "Match declined successfully" };
            }
            return new ServiceResult { Success = false, Message = "Match not found" };
        }

        public Task<IEnumerable<Match>> FindMatchesAsync(int userId, string interest)
        {
            throw new NotImplementedException();
        }
    }
}
