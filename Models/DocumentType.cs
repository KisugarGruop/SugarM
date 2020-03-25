using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class DocumentType : Maptable {
        public string CaneYear { get; set; }
        public string DocCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุพันธุ์อ้อย"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string DocName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุพันธุ์อ้อย"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; }

    }
}