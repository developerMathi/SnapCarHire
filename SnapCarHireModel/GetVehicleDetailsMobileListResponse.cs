﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel
{
    [Serializable]
    public class GetVehicleDetailsMobileListResponse
    {
        public List<VehicleTypeMobileResult> listVehicle { get; set; }

        public ApiMessage message { get; set; }
    }
}
