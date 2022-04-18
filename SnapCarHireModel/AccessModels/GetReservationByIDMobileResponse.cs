using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public class GetReservationByIDMobileResponse
    {
        public ReservationViewModel reservationData { get; set; }
        public VehicleTypeWithRatesViewModel vehicleTypeModel { get; set; }
        public VehicleWithRatesViewModel vehicleModel { get; set; }
        public bool isTimerVisible { get; set; }

        public ApiMessage message { get; set; }
    }
}
