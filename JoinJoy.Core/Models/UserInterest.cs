using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class UserInterest
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int InterestId { get; set; }
        public Interest Interest { get; set; }
        public int Weight { get; set; } // User-defined weight for ranking
    }
}
