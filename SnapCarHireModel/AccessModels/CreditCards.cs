using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    public class CreditCards
    {
        private string rawCreditCardNo;
        private string _CreditCardId;


        public int? CreditCardId { get; set; }
        public int? CustomerId { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNo
        {
            get { return _CreditCardId; }
            set
            {
                _CreditCardId = value;
                rawCreditCardNo = value;
            }
        }

        public string CreditCardNoForDisplay { get; set; }
        //public DateTimeOffset? CreditCardExpiryDate { get; set; }
        public DateTime? CreditCardExpiryDate { get; set; }
        public string CreditCardCVSNo { get; set; }

        public string CreditCardExpiryDateStr { get; set; }
        public string CreditCardBillingZipCode { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsviewCardNum { get; set; }

        public string NameOnCard { get; set; }

        public string CreditCard { get; set; }
        public int ReferenceId { get; set; }
        public int ReferenceTypeId { get; set; } //ReferenceTypeId --> 1:Insurance

        public string getRawCreditCardNo()
        { return rawCreditCardNo; }

    }
}
