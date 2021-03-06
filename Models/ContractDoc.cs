using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class ContractDoc : Maptablethen {
        [Required (ErrorMessage = "กรุณาระบุCompCode"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }
        public string CaneYear { get; set; }
        public string ContractCode { get; set; }
        public string DocCode { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";
        public string NameMap { get; set; }
        public string ContyeName { get; set; }
        public string DocName { get; set; }
        public List<ContractDoc> ContractDoclist { get; set; }
    }
}