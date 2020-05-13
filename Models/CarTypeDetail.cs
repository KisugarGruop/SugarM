using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class CarTypeDetail : Maptablethen
    {
        [Required(ErrorMessage = "กรุณาระบุรหัสCompCode"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string TypeCode { get; set; }
        public string SubTypeCode { get; set; }
        [Required(ErrorMessage = "กรุณาระบุDescription"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public decimal? WeightIn { get; set; }
        public decimal? WeightOut { get; set; }
        public decimal? TotalFuel { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string Fullname { get; set; }
        public List<CarTypeDetail> CarTypelist { get; set; }
    }
}