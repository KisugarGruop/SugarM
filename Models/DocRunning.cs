using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class DocRunning : Maptablethen {
        [Required (ErrorMessage = "กรุณาระบุรหัส"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string RunningId { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อ"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string RunningName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุRemark"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string RunningMark { get; set; }
        public string CompCode { get; set; }
        public string RunningYear { get; set; }
        public string SeparateChar { get; set; }
        public int DigitRunning { get; set; }
        public int LastRunning { get; set; }

    }
}