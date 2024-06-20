using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JoinJoy.Core.Models
{
    public class Location
    {
        public int Id { get; set; } // Unique identifier for the location
        public double? Latitude { get; set; } // Latitude coordinate for the location
        public double? Longitude { get; set; } // Longitude coordinate for the location
        public string Address { get; set; } // Address of the location
        public string? PlaceId { get; set; } // Google Place ID for precise location identification

        // Navigation properties
        public ICollection<Activity> Activities { get; set; } // Collection of activities at this location

    }
}
