using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class CaneType : Maptablethen {
        public string CompCode { get; set; }
        public string TypeCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุ(รายละเอียด)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Pass { get; set; }

    }
}