using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class ContractType : Maptable {
        public string CaneYear { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสประเภทสัญญา"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string ContractCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรายละเอียด"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
    }
}