using System.ComponentModel.DataAnnotations;
namespace SugarM.Models {
    public class Bank : Maptable {
        [Required (ErrorMessage = "กรุณาระบุรหัสธนาคาร"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BankCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อธนาคาร"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BankName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสสาขา"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BranchCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อสาขา"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BranchName { get; set; }
    }
}