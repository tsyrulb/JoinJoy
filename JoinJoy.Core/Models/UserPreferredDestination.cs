using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class UserPreferredDestination
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PreferredDestinationId { get; set; }
        public PreferredDestination PreferredDestination { get; set; }
        public int Weight { get; set; } // User-defined weight for ranking
    }
}
