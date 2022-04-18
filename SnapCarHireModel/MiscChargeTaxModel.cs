using System;

namespace SnapCarHireModel
{
    [Serializable]
    public class MiscChargeTaxModel
    {
        public string Name { get; set; }
        public int TaxId { get; set; }
        public int ClientId { get; set; }
        public int MisChargeId { get; set; }
        public decimal Value { get; set; }
        public string TaxSelectedStatus { get; set; } // Default empty / null
        //PP added to save individual tax rate & amount 
        public decimal taxAmount { get; set; }

    }

}
