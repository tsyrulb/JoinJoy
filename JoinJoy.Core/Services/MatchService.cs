using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Repositories;
using JoinJoy.Core.MatchingAlgorithms;

namespace JoinJoy.Core.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly WeightedInterestMatching _weightedInterestMatching;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
            _weightedInterestMatching = new WeightedInterestMatching();
        }

        public async Task<IEnumerable<Match>> FindMatchesAsync(int userId, string interest)
        {
            // Find matches using weighted interest matching
            var user = await _matchRepository.GetUserByIdAsync(userId);
            var allUsers = await _matchRepository.GetAllUsersAsync();
            var matches = _weightedInterestMatching.FindMatches(user, allUsers, interest);
            return matches;
        }
    }
}
