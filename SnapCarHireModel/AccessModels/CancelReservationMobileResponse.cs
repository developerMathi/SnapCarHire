using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public class CancelReservationMobileResponse
    {
        public string ReservationNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ApiMessage message { get; set; }
    }
}
