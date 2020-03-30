using System.Collections.Generic;

namespace SugarM.Models {
    public class ContractDoc : Maptable {
        public string CaneYear { get; set; }
        public string ContractCode { get; set; }
        public string DocCode { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; }
        public string NameMap { get; set; }
        public string ContyeName { get; set; }
        public string DocName { get; set; }
        public List<ContractDoc> ContractDoclist { get; set; }
    }
}