using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public class CancelReservationMobileRequest
    {
        public string reservationNo { get; set; }
        public DateTime CurrentTime { get; set; }
        public bool isTwoHour { get; set; }
    }
}
