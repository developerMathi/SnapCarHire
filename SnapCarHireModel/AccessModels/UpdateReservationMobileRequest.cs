using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public class UpdateReservationMobileRequest
    {
        public ReservationView reservationData { get; set; }

        public string BlackOutDays { get; set; }
    }
}
