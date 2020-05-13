using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class UserLevel : Maptablethen
    {
        [Required(ErrorMessage = "กรุณาระบุCompCode"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string CaneYear { get; set; }
        public string UserId { get; set; }
        public string UseridN { get; set; }

        [Required(ErrorMessage = "กรุณาระบุรหัสผู้อนุมัติ"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string UseridLevel { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public List<Override> Overrideslist { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string Fullname { get; set; }

    }
}