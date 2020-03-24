using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.TagHelpers;

namespace SugarM.Controllers
{
    public class CarBrandController : BaseController<CarBrand>
    {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser() => _userManager.GetUserName(HttpContext.User);

        public CarBrandController(IClientNotification clientNotification, UserManager<ApplicationUser> userManager)
        {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }

        [DisplayName("เพิ่มยี่ห้อ")]
        public IActionResult CreateCarBrand(string Id, string TypeId)
        {
            ViewBag.IsEditMode = "false";

            List<CarType> AuthorList = new List<CarType>();
            var _ReCarType = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorList, "http://192.168.10.46/sdapi/sdapi/CarBrandGet/" + Id + "/" + TypeId, Getkey());
            var CarType = new List<CarType>();
            foreach (var sname in _ReCarType)
            {
                CarType.Add(new CarType
                {
                    TypeCode = sname.TypeCode,
                    Fullname = sname.TypeCode + " | " + sname.Description
                });
            }
            List<CarBrand> AuthorListdetail = new List<CarBrand>();
            CarBrand _CarBrand = new CarBrand();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarBrandGet/" + Id + "/" + TypeId, Getkey());
            foreach (var item in Call)
            {
                _CarBrand.CompCode = item.CompCode;
                _CarBrand.TypeCode = item.TypeCode;
                _CarBrand.Description = "";
                _CarBrand.Active = item.Active;

            }
            ViewBag.Car = CarType;
            _CarBrand.CarBranList = Call;
            return View("CreateCarBrand", _CarBrand);
        }

        [DisplayName("แก้ประเภทย่อย")]
        [HttpGet]
        public IActionResult EditDetaill(string Id, string TypeId, string BrandCode)
        {
            ViewBag.IsEditMode = "true";
            List<CarType> AuthorList = new List<CarType>();
            var _ReCarType = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorList, "http://192.168.10.46/sdapi/sdapi/CarBrandGet/" + Id + "/" + TypeId, Getkey());
            var CarType = new List<CarType>();
            foreach (var sname in _ReCarType)
            {
                CarType.Add(new CarType
                {
                    TypeCode = sname.TypeCode,
                    Fullname = sname.TypeCode + " | " + sname.Description
                });
            }
            List<CarBrand> AuthorListdetail = new List<CarBrand>();
            CarBrand _CarBrand = new CarBrand();
            //*** หาค่า CarBrand เอามาทำ datatable 
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarBrandGet/" + Id + "/" + TypeId, Getkey());
            //* หาค่าที่จะเอามาแก้ไข
            var CarBrandgetdetaill = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarBrandGet/" + Id + "/" + TypeId + "/" + BrandCode, Getkey());
            foreach (var item in CarBrandgetdetaill)
            {
                _CarBrand.CompCode = item.CompCode;
                _CarBrand.TypeCode = item.TypeCode;
                _CarBrand.BrandCode = item.BrandCode;
                _CarBrand.BrandName = item.BrandName;
                _CarBrand.Description = "";
                _CarBrand.Active = item.Active;

            }
            ViewBag.Car = CarType;
            _CarBrand.CarBranList = Call;
            return View("CreateCarBrand", _CarBrand);
        }

        [HttpPost]
        [DisplayName("บันทึกประเภทย่อย")]
        public IActionResult SaveCarBrand(CarBrand _Carbrand, string IsEditMode)
        {
            var UserCurrent = GetCurrentUser();
            if (IsEditMode.Equals("false"))
            {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_Carbrand, "http://192.168.10.46/sdapi/sdapi/CarBrandPost", Getkey());
                if (_Re.success)
                {
                    _clientNotification.AddSweetNotification("สำเร็จ",
                        "บันทึกข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                }
                else
                {
                    _clientNotification.AddSweetNotification("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                }
            }
            else
            {
                CarBrand _CarBrandUP = new CarBrand()
                {
                    BrandCode = _Carbrand.BrandCode,
                    BrandName = _Carbrand.BrandName,
                    Active = _Carbrand.Active,
                    DeleteFlag = _Carbrand.DeleteFlag,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime(DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_CarBrandUP, "http://192.168.10.46/sdapi/sdapi/CarBrandPut/" + _Carbrand.CompCode + "/" + _Carbrand.TypeCode + "/" + _Carbrand.BrandCode, Getkey());
                if (_Re.success)
                {
                    _clientNotification.AddSweetNotification("สำเร็จ",
                        "แก้ไขข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                    return RedirectToAction(nameof(CreateCarBrand), new { Id = _Carbrand.CompCode, TypeId = _Carbrand.TypeCode });
                }
                else
                {
                    _clientNotification.AddSweetNotification("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                }
            }
            return RedirectToAction(nameof(CreateCarBrand), new { Id = _Carbrand.CompCode, TypeId = _Carbrand.TypeCode });
        }

        [DisplayName("ลบประเภทย่อย")]
        [HttpGet]
        public IActionResult Delete(string Id, string Del, string Del1)
        {
            SaleAuth _SaleAuth = new SaleAuth();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST(_SaleAuth, "http://192.168.10.46/sdapi/sdapi/CarBrandDel/" + Id + "/" + Del + "/" + Del1, Getkey());
            return Json(new { success = Call.success });
        }
    }
}