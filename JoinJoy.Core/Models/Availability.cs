using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class Availability
    {
        public int Id { get; set; } // Unique identifier for the availability
        public string TimeOfDay { get; set; } // General time availability (e.g., Morning, Afternoon, Evening)
        public string DaysOfWeek { get; set; } // Days of the week available (e.g., Monday, Tuesday)
        public string Seasons { get; set; } // Seasonal availability (e.g., Winter, Summer)

        // Navigation property
        public ICollection<UserAvailability> UserAvailabilities { get; set; }
    }
}
