using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.MatchingAlgorithms
{
    public class CollaborativeFiltering
    {
        public List<User> CollaborativeFilteringMatches(User currentUser, List<User> allUsers)
        {
            var similarities = new List<(User user, double similarityScore)>();
            foreach (var user in allUsers)
            {
                if (user != currentUser)
                {
                    double similarityScore = CalculateSimilarity(currentUser, user);
                    similarities.Add((user, similarityScore));
                }
            }
            return similarities.OrderByDescending(s => s.similarityScore).Take(5).Select(s => s.user).ToList();
        }

        private double CalculateSimilarity(User currentUser, User user)
        {
            // Simplified similarity calculation
            // return currentUser.Interests.Intersect(user.Interests).Count() / (double)currentUser.Interests.Count();
            return 0;
        }
    }
}