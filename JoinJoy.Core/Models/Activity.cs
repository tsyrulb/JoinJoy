using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JoinJoy.Core.Models
{
    public class Activity
    {
        [Key]
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Location Location { get; set; }
    }
}
