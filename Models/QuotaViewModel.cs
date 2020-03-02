using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class QuotaViewModel {
        [Required (ErrorMessage = "กรุณาระบุประเภทโควต้า"), MinLength (2, ErrorMessage = "Minimum length is 2.")]

        public string TypeCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรายละเอียด"), MinLength (2, ErrorMessage = "Minimum length is 2.")]

        public string Description { get; set; }
    }
}