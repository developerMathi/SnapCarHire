using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel
{
    public class VehicleWithRatesViewModel
    {
        public string VehicleNo { get; set; }
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public int CurrentOdometer { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VehicleType { get; set; }
        public string Trim { get; set; }
        public string LicenseNo { get; set; }
        public string VIN { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public string Transmission { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public double Rate { get; set; }
        public double TotalRateCharge { get; set; }
        public int? CurrentLocationId { get; set; }
        public string LocationName { get; set; }
        public bool IsAvailable { get; set; }
        public string Seats { get; set; }
        public int Baggages { get; set; }
        public string FuelType { get; set; }
        public int TankSize { get; set; }
    }
}
