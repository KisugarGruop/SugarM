using System.Collections.Generic;

namespace SugarM.Models {
    public class CarBrand : Maptable {
        public string TypeCode { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; }
        public string Fullname { get; set; }
        public List<CarBrand> CarBranList { get; set; }
    }
}