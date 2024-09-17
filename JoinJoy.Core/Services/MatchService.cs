using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> GetUserMatchesAsync(int userId)
        {
            return await _matchRepository.GetUserMatchesAsync(userId);
        }

        public async Task<ServiceResult> CreateMatchAsync(Match match)
        {
            await _matchRepository.AddAsync(match);
            return new ServiceResult { Success = true, Message = "Match created successfully" };
        }

        public async Task<ServiceResult> AcceptMatchAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
            {
                return new ServiceResult { Success = false, Message = "Match not found" };
            }

            match.IsAccepted = true;
            await _matchRepository.UpdateAsync(match);
            return new ServiceResult { Success = true, Message = "Match accepted" };
        }
    }
}
