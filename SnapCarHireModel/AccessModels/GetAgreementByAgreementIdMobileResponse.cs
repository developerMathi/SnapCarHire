using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    public class GetAgreementByAgreementIdMobileResponse
    {

        //    public CustomerAgreementModel custAgreement { get; set; }
        public AgreementReviewDetailSet custAgreement { get; set; }
        public VehicleWithRatesViewModel agreementVehicle { get; set; }
        public ApiMessage message { get; set; }
    }
}
