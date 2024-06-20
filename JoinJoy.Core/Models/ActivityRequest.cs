using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class ActivityRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string LocationName { get; set; }
        public int CreatedById { get; set; }
    }
}
