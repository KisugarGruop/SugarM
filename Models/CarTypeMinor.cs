using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class CarTypeMinor : Maptablethen
    {
        [Required(ErrorMessage = "กรุณาระบุรหัสCompCode"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string TypeCode { get; set; }
        public string SubTypeCode { get; set; }
        public string MinorTypeCode { get; set; }
        [Required(ErrorMessage = "กรุณาระบุDescription"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string Description { get; set; }
        public decimal WeightIn { get; set; }
        public decimal WeightOut { get; set; }
        public decimal TotalFuel { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string CarTypeDesc { get; set; }
        public string CarTypeDetailDesc { get; set; }
        public string SubFullname { get; set; }
        public string TypeFullname { get; set; }
        public List<CarTypeMinor> CarTypeMinorlist { get; set; }
    }
}