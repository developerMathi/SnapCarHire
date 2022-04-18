using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    public class AddLicenceImagesRequest
    {
        public LicenceImage FrontImage { get; set; }
        public LicenceImage BackImage { get; set; }
    }

    public class LicenceImage
    {
        public string base64DocumentContent { get; set; }
        public string contentType { get; set; }
        public int refId { get; set; }
        public short refType { get; set; }
        public string docName { get; set; }
    }
}
