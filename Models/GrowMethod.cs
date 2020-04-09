using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class GrowMethod : Maptable {
        public string CaneYear { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสการปลูก"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string GrowCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อ"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
    }
}