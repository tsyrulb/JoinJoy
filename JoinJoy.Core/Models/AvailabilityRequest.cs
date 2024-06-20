namespace JoinJoy.Core.Models
{
    public class AvailabilityRequest
    {
        public int? Id { get; set; } // This will be null for new availabilities and have a value for updates
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
