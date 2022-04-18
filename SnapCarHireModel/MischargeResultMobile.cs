using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapCarHireModel.AccessModels
{
    [Serializable]
    public class MischargeResultMobile
    {

        public int VehicleTypeId { get; set; }

        public int LocationId { get; set; }

        public int MiscChargeID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CalculationType { get; set; }

        public decimal Value { get; set; }

        public int MisChargeCode { get; set; }

        public bool IsOptional { get; set; }

        public int Unit { get; set; }

        public bool TaxNotAvailable { get; set; }

        public bool IsQuantity { get; set; }

        public bool isSelected { get; set; }

        public string ViewString { get; set; }

        public bool IsMantatory { get { return !IsOptional; } }

        public decimal price
        {
            get
            {
                if (IsQuantity)
                {
                    return Value * Unit;
                }
                else
                {
                    return Value;
                }
            }

        }
    }
}
