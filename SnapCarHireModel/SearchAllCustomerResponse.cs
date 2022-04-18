using SnapCarHireModel.AccessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel
{
    public class SearchAllCustomerResponse
    {
        public List<CustomerSeachResult> serachResult { get; set; }
        public ApiMessage message { get; set; }
    }
}
