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
        public DayOfWeek DayOfWeek { get; set; } // Enum to represent the day of the week
        public TimeSpan StartTime { get; set; } // Start time of availability
        public TimeSpan EndTime { get; set; } // End time of availability
        public ICollection<UserAvailability> UserAvailabilities { get; set; } // Many-to-many relationship with users
    }
}