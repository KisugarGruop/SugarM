using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class CaneQlt : Maptablethen
    {
        [Required(ErrorMessage = "กรุณาระบุรหัสCompCode"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string CaneYear { get; set; }
        public string Quality { get; set; }
        [Required(ErrorMessage = "กรุณาระบุDescription"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public string Pass { get; set; }

    }
}