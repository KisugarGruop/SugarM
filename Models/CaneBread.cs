using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class CaneBread : Maptable {
        [Required (ErrorMessage = "กรุณาระบุพันธุ์อ้อย"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BreadName { get; set; }
        public string BreadWeight { get; set; }
        public string remarks { get; set; }

    }
}