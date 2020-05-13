using System.ComponentModel.DataAnnotations;
namespace SugarM.Models {
    public class Bank : Maptablethen {
        [Required (ErrorMessage = "กรุณาระบุรหัสธนาคาร"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BankCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อธนาคาร"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BankName { get; set; }
    }
}