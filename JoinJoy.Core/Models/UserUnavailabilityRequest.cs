using System;

namespace JoinJoy.Core.Models
{
    public class UserUnavailabilityRequest
    {
        public int DayOfWeek { get; set; } // 0 (Sunday) to 6 (Saturday)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
