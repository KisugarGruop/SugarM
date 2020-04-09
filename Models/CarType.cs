using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class CarType : Maptable {
        [Required (ErrorMessage = "กรุณาระบุรหัส(TypeCode)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string TypeCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อประเภทรถ(Description)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string Fullname { get; set; }
    }
}