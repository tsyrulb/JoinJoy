using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> FindMatchesAsync(int userId, string interest);
        // Additional service methods
    }
}
