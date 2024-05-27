using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.MatchingAlgorithms
{
    public class WeightedInterestMatching
    {
        public IEnumerable<Match> FindMatches(User currentUser, IEnumerable<User> allUsers, string interest)
        {
            var matches = new List<Match>();
            foreach (var user in allUsers)
            {
                if (user.Id != currentUser.Id )
                {
                    var compatibilityScore = 0; //CalculateCompatibility(currentUser, user);
                    if (compatibilityScore > 0.5) // Threshold for matching
                    {
                        matches.Add(new Match
                        {
                            User1 = currentUser,
                            User2 = user,
                            MatchDate = System.DateTime.Now
                        });
                    }
                }
            }
            return matches;
        }

        // private double CalculateCompatibility(User currentUser, User user)
        // {
        // double compatibilityScore = 0;
        //     foreach (var interest in currentUser.Interests.Split(','))
        //      {
        //          if (user.Interests.Contains(interest))
        //          {
        //  compatibilityScore += 1; // Simplified compatibility calculation
        // }
        //  }
        //      return compatibilityScore / currentUser.Interests.Split(',').Length;
        // }
    }
}