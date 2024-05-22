using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.MatchingAlgorithms
{
    public class FeedbackIntegration
    {
        public void UpdateMatchQuality(User user1, User user2, bool positiveFeedback)
        {
            if (positiveFeedback)
            {
                IncreaseCompatibilityScore(user1, user2);
            }
            else
            {
                DecreaseCompatibilityScore(user1, user2);
            }
        }

        private void IncreaseCompatibilityScore(User user1, User user2)
        {
            // Simplified logic for increasing compatibility score
            user1.Interests += ",PositiveFeedback";
            user2.Interests += ",PositiveFeedback";
        }

        private void DecreaseCompatibilityScore(User user1, User user2)
        {
            // Simplified logic for decreasing compatibility score
            user1.Interests += ",NegativeFeedback";
            user2.Interests += ",NegativeFeedback";
        }
    }
}