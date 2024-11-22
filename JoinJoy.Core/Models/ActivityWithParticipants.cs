using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class ActivityWithParticipants
    {
        public int ActivityId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Location { get; set; }

        public int? CreatedById { get; set; }
        public IEnumerable<ParticipantInfo>? Participants { get; set; }
    }

    public class ParticipantInfo
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }

        public string? PictureUrl { get; set; }
    }

}
