using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public partial class EmailInvoiceRequest
    {
        public int ClientID { get; set; }
        public int TemplateTypeId { get; set; }
        public int PurposeId { get; set; }
        public int? AgreementId { get; set; }
        public int? ReservationId { get; set; }
    }
}
