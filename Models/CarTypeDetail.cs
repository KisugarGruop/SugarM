using System.Collections.Generic;

namespace SugarM.Models
{
    public class CarTypeDetail : Maptable
    {
        public string TypeCode { get; set; }
        public string SubTypeCode { get; set; }
        public string Description { get; set; }
        public decimal? WeightIn { get; set; }
        public decimal? WeightOut { get; set; }
        public decimal? TotalFuel { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; }
        public string Fullname { get; set; }
        public List<CarTypeDetail> CarTypelist { get; set; }
    }
}