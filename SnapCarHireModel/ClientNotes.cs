using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel
{
    public class ClientNotes
    {
        public int ClientNoteId { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}
