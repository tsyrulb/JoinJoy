using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinJoy.Core.Models
{
    public class UserUnavailability
    {
        public int Id { get; set; } // Unique identifier for unavailability period

        [ForeignKey("User")]
        public int UserId { get; set; } // Reference to the user
        public User User { get; set; } // Navigation property

        public int DayOfWeek { get; set; } // Day of the week (0 = Sunday, 6 = Saturday)

        [Required]
        public TimeSpan StartTime { get; set; } // Start time of unavailability

        [Required]
        public TimeSpan EndTime { get; set; } // End time of unavailability
    }
}
