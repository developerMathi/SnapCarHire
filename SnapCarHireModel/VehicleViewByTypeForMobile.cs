using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel
{
    public class VehicleViewByTypeForMobile
    {
        public int VehicleTypeId { get; set; }
        public string VehicleType { get; set; }
        public string Transmission { get; set; }
        public int Seats { get; set; }
        public string VehicleImageUrl { get; set; }
        public string VehicleTypeImageUrl { get; set; }
        public string sample { get; set; }
        public int NoOfLuggage { get; set; }
        public string doors { get; set; }
        public decimal? DailyRate { get; set; }
        public decimal? DailyRateWithInsurance { get; set; }
        public decimal? MileagePerDay { get; set; }


        public RateDetail RateDetail { get; set; }
        public bool IsVehicleAvailable { get; set; }
        public string IsVehicleAvailableDescription { get; set; }
        public string HtmlContent { get; set; }
        public string SharableLink { get; set; }

        public List<int> locationIdList { get; set; }


        public int vehicleId { get; set; }
        public string vehicleName { get; set; }
        public string vehicleMakeName { get; set; }
        public string vehicleModelName { get; set; }
        public int vehicleYear { get; set; }
        public string color { get; set; }

        public List<ReservationMiscDetails> mantatoryMiscChargeDetails { get; set; }
        public decimal mantatoryMiscChargeTotalForOneDay { get; set; }
        public decimal mantatoryMiscChargeNonTaxableTotalForOneDay { get; set; }






    }
}
