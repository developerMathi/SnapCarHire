using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel
{
    public class CutomerAuthContext : CutomerContext
    {

        public string ClientCurrencySymbol { get; set; }
        public string ClientCurrency { get; set; }
        public string userCultureInfo { get; set; }
        public string TimeZone { get; set; }
        public string UICulture { get; set; }
        public string Culture { get; set; }
        public string MasterPage { get; set; }
        public int MaxNoOfUser { get; set; }
        public int MaxNoOfVehicles { get; set; }
        public double UsedFileSize { get; set; }
        public double MaxFileSize { get; set; }
        public string Theme { get; set; }
        public string CustomizationFolder { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string RoleName { get; set; }
        public int UserRoleID { get; set; }
        public string ConsumerType { get; set; }
        public string CustomerName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public int Country { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int UserGroupID { get; set; }
        public string CustomerEmail { get; set; }
    }
}