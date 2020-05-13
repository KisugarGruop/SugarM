using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class CaneBread : Maptablethen
    {
        [Required(ErrorMessage = "กรุณาระบุCompCode"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        [Required(ErrorMessage = "กรุณาระบุพันธุ์อ้อย"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string BreadName { get; set; }
        public string BreadWeight { get; set; }
        public string remarks { get; set; }

    }
}