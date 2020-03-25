using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Unit : Maptable {
        [Required (ErrorMessage = "กรุณาระบุชื่อ"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string UnitName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรายละเอียด"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; }
    }
}