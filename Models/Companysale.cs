using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Companysale : Maptablethen {
        public string SaleId { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อ(SaleName)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string SaleName { get; set; }
        public string CompCode { get; set; }
        public string BranchCode { get; set; }
        public string NameTH { get; set; }
        public string DpComIdandname { get; set; }
    }
}