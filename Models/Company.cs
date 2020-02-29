using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Company : Maptable {
        public string CurrCaneYear { get; set; }
        public string DistFrPlant { get; set; }
        public string FisCompCode { get; set; }
        public string FisBranchCode { get; set; }
        public string LicenseKey { get; set; }
        public string Latdd { get; set; }
        public string Longdd { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสสาขา(BranchCode)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string BranchCode { get; set; }

        public string BranchType { get; set; }
        public string RegionCode { get; set; }
        public string Head { get; set; }
        public string DefaultWHCode { get; set; }
        public string DefaultLocID { get; set; }
        public string BranchSymbool { get; set; }
        public string CfsUserId { get; set; }
        public string LeasingContractId { get; set; }
        public string LoanContractId { get; set; }
        public string GuaranteeContractId { get; set; }
        public string CaneContractId { get; set; }
        public int BranchTypeId { get; set; }

    }
    public class combanchty {
        public int Id { get; set; }
        public string BranchType { get; set; }
    }
}