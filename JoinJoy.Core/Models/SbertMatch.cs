using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class SbertMatch
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public float Similarity { get; set; }
    }

    public class SbertMatchResponse
    {
        public List<SbertMatch> Top_Matches { get; set; }
    }

}
