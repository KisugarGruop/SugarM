using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class QuotaType : Maptable {
        public string TypeCode { get; set; }

        public string Description { get; set; }
    }
}