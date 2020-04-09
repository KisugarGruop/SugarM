using System.Collections.Generic;

namespace SugarM.Models {
    public class CarTypeMinor : Maptable {
        public string ComCode { get; set; }
        public string TypeCode { get; set; }
        public string SubTypeCode { get; set; }
        public string MinorTypeCode { get; set; }
        public string Description { get; set; }
        public decimal WeightIn { get; set; }
        public decimal WeightOut { get; set; }
        public decimal TotalFuel { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string CarTypeDesc { get; set; }
        public string CarTypeDetailDesc { get; set; }
        public List<CarTypeMinor> CarTypeMinorlist { get; set; }
    }
}