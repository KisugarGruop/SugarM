using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarM.Models
{
    public class DocCtrRunno : Maptable
    {
        public string CaneYear { get; set; }
        public string DocCode { get; set; }
        public string DocType { get; set; }
        public string BranchSymbol { get; set; }
        public int LastRunNo { get; set; }
        public int DigitRunNo { get; set; }
        [Required(ErrorMessage = "กรุณาระบุชื่อ"), MinLength(2, ErrorMessage = "Minimum length is 2.")]
        public string CodeName { get; set; }
        public string TypeName { get; set; }
        public string BranchCode { get; set; }
    }
}