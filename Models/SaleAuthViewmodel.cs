using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class SaleAuthViewmodel : Maptable {
        [Required (ErrorMessage = "กรุณาระบุรหัสนักเกษตร"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string SaleId { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสเขต"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string RegionCode { get; set; }
        public string Position { get; set; }
        public string SaleName { get; set; }
        public string SaleFullname { get; set; }
        public string RegCodeAndName { get; set; }
        public List<SaleAuth> Salelist { get; set; }

        //*------------- เอาไว้เก็บ ค่า pk ตัวเก่า เพื่อที่จะเอาไปแก้ไข
        public string SaleIdEdit { get; set; }
        public string RegionCodeEdit { get; set; }
        public string CompCodeEdit { get; set; }
    }
}