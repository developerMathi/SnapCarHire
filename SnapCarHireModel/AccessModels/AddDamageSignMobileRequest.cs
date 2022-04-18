using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    public class AddDamageSignMobileRequest
    {
        public int agreementId { get; set; }
        public string base64Img { get; set; }
        public bool ischeckin { get; set; }

    }
}
