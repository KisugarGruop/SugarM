using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Override : Maptablethen {
        [Required (ErrorMessage = "กรุณาระบุCompCode"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string CaneYear { get; set; }

        [Required (ErrorMessage = "กรุณาระบุระดับผู้อนุมัติ"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string OverrideLevel { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string Description { get; set; }
        public string Active { get; set; }

    }
}