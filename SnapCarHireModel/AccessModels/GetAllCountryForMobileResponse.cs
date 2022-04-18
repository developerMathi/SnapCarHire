using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GetAllCountryForMobileResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Country> countryList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ApiMessage message { get; set; }
    }
}
