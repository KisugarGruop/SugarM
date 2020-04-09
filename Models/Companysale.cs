using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Companysale : Maptable {
        public string SaleId { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อ(SaleName)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string SaleName { get; set; }
        public string BranchCode { get; set; }
        public string DpComIdandname { get; set; }
    }
}