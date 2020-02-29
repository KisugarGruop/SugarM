using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SugarM.Models {
    public class SaleAuth : Maptable {
        public string SaleId { get; set; }
        public string RegionCode { get; set; }
        public string Position { get; set; }
        public string SaleName { get; set; }
        public string SaleFullname { get; set; }
        public string RegCodeAndName { get; set; }
        //*------------- เอาไว้เก็บ ค่า pk ตัวเก่า เพื่อที่จะเอาไปแก้ไข
        public string SaleIdEdit { get; set; }
        public string RegionCodeEdit { get; set; }
        public string CompCodeEdit { get; set; }
    }
}