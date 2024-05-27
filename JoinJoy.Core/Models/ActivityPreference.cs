using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class ActivityPreference
    {
        public int Id { get; set; } // Unique identifier for the activity preference
        public string Category { get; set; } // Category of activity (e.g., Social, Educational, Fitness)
        public string Name { get; set; } // Name of the activity (e.g., Concerts, Yoga Classes)

        // Navigation property
        public ICollection<UserActivityPreference> UserActivityPreferences { get; set; }
    }
}
