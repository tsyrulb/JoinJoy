using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class Hobby
    {
        public int Id { get; set; } // Unique identifier for the hobby
        public string Type { get; set; } // Type of hobby (e.g., Indoor, Outdoor, Creative)
        public string Name { get; set; } // Name of the hobby (e.g., Cooking, Hiking)

        // Navigation property
        public ICollection<User> Users { get; set; } // Users who have this hobby
    }
}
