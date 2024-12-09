using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class UserRecommendation
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public double SimilarityScore { get; set; }

        public double Distance { get; set; }
        public string Explanation { get; set; }
    }
}
