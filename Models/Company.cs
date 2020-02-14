using System.ComponentModel.DataAnnotations;

namespace SugarM.Models {
    public class Company {
        public string Statusform { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสบริษัท(CompCode)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string CompCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อบริษัท(NameTH)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string NameTH { get; set; }
        public string NameEN { get; set; }

        [Required (ErrorMessage = "ระบุที่อยุ่(บ้านเลขที่-หมู่ที่-ตำบล)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Addr1 { get; set; }

        [Required (ErrorMessage = "กรุณาระบุอำเภอ"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Addr2 { get; set; }

        [Required (ErrorMessage = "กรุณาระบุจังหวัด"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Addr3 { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสไปรษณีย์(ZIP)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string ZIP { get; set; }

        [Required (ErrorMessage = "กรุณาระบุเบอร์โทร(Tel)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Tel { get; set; }

        [Required (ErrorMessage = "กรุณาระบุอีเมล์(Email)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Fax { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string TaxID { get; set; }
        public string RegID { get; set; }
        public string RegDate { get; set; }
        public string CurrCaneYear { get; set; }
        public string DistFrPlant { get; set; }
        public string FisCompCode { get; set; }
        public string FisBranchCode { get; set; }
        public string LicenseKey { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public string Version { get; set; }
        public string Latdd { get; set; }
        public string Longdd { get; set; }

    }
}