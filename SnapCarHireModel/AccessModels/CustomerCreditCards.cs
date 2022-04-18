using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public partial class CustomerCreditCards
    {
        public CustomerCreditCards()
        {
        }
        public int CreditCardId { get; set; }
        public int CustomerId { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNo { get; set; }
        public DateTime? CreditCardExpiryDate { get; set; }
        public string CreditCardCVSNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Int16 IsDeleted { get; set; }
        public string CreditCardBillingZipCode { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public string CreditCardExpiryDateStr { get; set; }
        public string NameOnCard { get; set; }
        public string Token { get; set; }

    }
}
