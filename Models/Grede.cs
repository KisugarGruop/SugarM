using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Grade : Maptablethen {
        [Required (ErrorMessage = "กรุณาระบุCompCode"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string CaneYear { get; set; }

        [Required (ErrorMessage = "กรุณาระบุเกรดชั้นหนี้"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string GradeCode { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string Description { get; set; }
        public string Active { get; set; }
    }
}