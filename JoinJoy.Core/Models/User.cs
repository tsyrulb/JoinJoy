using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JoinJoy.Core.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string ProfilePhoto { get; set; }
        public string Interests { get; set; }  // JSON string
        public string Hobbies { get; set; }  // JSON string
        public string Activities { get; set; }  // JSON string
        public string PreferredDestinations { get; set; }  // JSON string
        public string Availability { get; set; }  // JSON string
        public int DistanceWillingToTravel { get; set; }
        public Location Location { get; set; }
    }
}