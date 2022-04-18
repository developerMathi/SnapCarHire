using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxVonGrafKftMobileModel.AccessModels
{
    public class ConfirmEmailAddressRequest
    {

        public string Email { get; set; }
        public int CustomerId { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public int ClientId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEmailSuccess { get; set; }
    }
}