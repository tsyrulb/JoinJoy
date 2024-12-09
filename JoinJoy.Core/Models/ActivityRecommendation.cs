using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class ActivityRecommendation
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public double SimilarityScore { get; set; }
        public double Distance { get; set; }
        public string Explanation { get; set; }

    }
    public class ActivityWithExplanation
    {
        public Activity Activity { get; set; }
        public string Explanation { get; set; }
    }
}
