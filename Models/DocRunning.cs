using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class DocRunning : Maptable
    {
        [Required(ErrorMessage = "กรุณาระบุรหัส"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string RunningId { get; set; }
        [Required(ErrorMessage = "กรุณาระบุชื่อ"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string RunningName { get; set; }
        public string RunningMark { get; set; }
        public string RunningYear { get; set; }
        public string SeparateChar { get; set; }
        public int DigitRunning { get; set; }
        public int LastRunning { get; set; }

    }
}