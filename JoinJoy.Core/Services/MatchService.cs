using JoinJoy.Core.Models;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.Core.Services
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> _matchRepository;

        public MatchService(IRepository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> FindMatchesAsync(int userId, string interest)
        {
            // Implement find matches logic here
            return new List<Match>();
        }
    }
}
