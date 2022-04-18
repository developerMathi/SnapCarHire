using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    public class ExtendAgreementResponse
    {
        public AgreementReview agreementReview { get; set; }
        public ApiMessage message { get; set; }
    }
}
