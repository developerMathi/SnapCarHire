﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    public class GetTermsandConditionByTypeRequest
    {
        public int clientId { get; set; }
        public int typeId { get; set; }
    }
}
