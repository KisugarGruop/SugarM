using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Models {
    public class User {
        [Required (ErrorMessage = "กรุณาระบุชื่อเข้าใช้งาน(UserName)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string UserName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุรหัสพนักงาน"), MinLength (6, ErrorMessage = "Minimum length is 6.")]
        public string EmployeeId { get; set; }
        public string CompCode { get; set; }

        [Required (ErrorMessage = "กรุณาระบุชื่อ"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุนามสกุล"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string LastName { get; set; }

        [Required (ErrorMessage = "กรุณาระบุอีเมล์"), EmailAddress]
        public string Email { get; set; }

        [DataType (DataType.Password), Required (ErrorMessage = "กรุณาระบุรหัสผ่าน (Password)"), MinLength (2, ErrorMessage = "Minimum length is 2.")]
        public string Password { get; set; }
    }
}