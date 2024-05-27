using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class PreferredDestination
    {
        public int Id { get; set; } // Unique identifier for the preferred destination
        public string Type { get; set; } // Type of destination (e.g., Travel Destination, Activity-Specific Destination)
        public string Name { get; set; } // Name of the destination (e.g., Paris, Local Gallery)

        // Navigation property
        public ICollection<UserPreferredDestination> UserPreferredDestinations { get; set; }
    }
}
