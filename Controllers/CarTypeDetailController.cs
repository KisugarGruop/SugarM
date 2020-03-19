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
    public class CarTypeDetailController : BaseController<CarTypeDetail>
    {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser() => _userManager.GetUserName(HttpContext.User);

        public CarTypeDetailController(IClientNotification clientNotification, UserManager<ApplicationUser> userManager)
        {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }

        [DisplayName("เพิ่มประเภทย่อย")]
        public IActionResult CreateDetaill(string Id, string TypeId)
        {
            ViewBag.IsEditMode = "false";

            List<CarType> AuthorList = new List<CarType>();
            var _ReCarType = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorList, "http://192.168.10.46/sdapi/sdapi/CarTypeGet/" + Id + "/" + TypeId, Getkey());
            var CarType = new List<CarType>();
            foreach (var sname in _ReCarType)
            {
                CarType.Add(new CarType
                {
                    TypeCode = sname.TypeCode,
                    Fullname = sname.TypeCode + " | " + sname.Description
                });
            }
            List<CarTypeDetail> AuthorListdetail = new List<CarTypeDetail>();
            CarTypeDetail _CarTypeDetaill = new CarTypeDetail();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailGet/" + Id + "/" + TypeId, Getkey());
            foreach (var item in Call)
            {
                _CarTypeDetaill.CompCode = item.CompCode;
                _CarTypeDetaill.TypeCode = item.TypeCode;
                _CarTypeDetaill.Description = "";
                _CarTypeDetaill.Active = item.Active;

            }
            ViewBag.Car = CarType;
            _CarTypeDetaill.CarTypelist = Call;
            return View("CreateDetaill", _CarTypeDetaill);
        }

        [DisplayName("แก้ประเภทย่อย")]
        [HttpGet]
        public IActionResult EditDetaill(string Id, string TypeId, string SubId)
        {
            ViewBag.IsEditMode = "true";

            List<CarType> AuthorList = new List<CarType>();
            var _ReCarType = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorList, "http://192.168.10.46/sdapi/sdapi/CarTypeGet/" + Id + "/" + TypeId, Getkey());
            var CarType = new List<CarType>();
            foreach (var sname in _ReCarType)
            {
                CarType.Add(new CarType
                {
                    TypeCode = sname.TypeCode,
                    Fullname = sname.TypeCode + " | " + sname.Description
                });
            }
            List<CarTypeDetail> AuthorListdetail = new List<CarTypeDetail>();
            CarTypeDetail _CarTypeDetaill = new CarTypeDetail();
            //*** หาค่า cardetaill เอามาทำ datatable 
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailGet/" + Id + "/" + TypeId, Getkey());
            //* หาค่าที่จะเอามาแก้ไข
            var Cargetdetaill = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailGet/" + Id + "/" + TypeId + "/" + SubId, Getkey());
            foreach (var item in Cargetdetaill)
            {
                _CarTypeDetaill.CompCode = item.CompCode;
                _CarTypeDetaill.TypeCode = item.TypeCode;
                _CarTypeDetaill.SubTypeCode = item.SubTypeCode;
                _CarTypeDetaill.Description = item.Description;
                _CarTypeDetaill.WeightIn = item.WeightIn;
                _CarTypeDetaill.WeightOut = item.WeightOut;
                _CarTypeDetaill.TotalFuel = item.TotalFuel;
                _CarTypeDetaill.Active = item.Active;

            }
            ViewBag.Car = CarType;
            _CarTypeDetaill.CarTypelist = Call;
            return View("CreateDetaill", _CarTypeDetaill);
        }

        [HttpPost]
        [DisplayName("บันทึกประเภทย่อย")]
        public IActionResult SaveCarTypeDetaill(CarTypeDetail _Cardetaill, string IsEditMode)
        {
            var UserCurrent = GetCurrentUser();
            if (IsEditMode.Equals("false"))
            {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_Cardetaill, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailPost", Getkey());
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
                CarTypeDetail _CardetaillUP = new CarTypeDetail()
                {
                    SubTypeCode = _Cardetaill.SubTypeCode,
                    Description = _Cardetaill.Description,
                    WeightIn = _Cardetaill.WeightIn,
                    WeightOut = _Cardetaill.WeightOut,
                    TotalFuel = _Cardetaill.TotalFuel,
                    Active = _Cardetaill.Active,
                    DeleteFlag = _Cardetaill.DeleteFlag,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime(DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_CardetaillUP, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailPut/" + _Cardetaill.CompCode + "/" + _Cardetaill.TypeCode + "/" + _Cardetaill.SubTypeCode, Getkey());
                if (_Re.success)
                {
                    _clientNotification.AddSweetNotification("สำเร็จ",
                        "แก้ไขข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                    return RedirectToAction(nameof(CreateDetaill), new { Id = _Cardetaill.CompCode, TypeId = _Cardetaill.TypeCode });
                }
                else
                {
                    _clientNotification.AddSweetNotification("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                }
            }
            return RedirectToAction(nameof(CreateDetaill), new { Id = _Cardetaill.CompCode, TypeId = _Cardetaill.TypeCode });
        }

        [DisplayName("ลบประเภทย่อย")]
        [HttpGet]
        public IActionResult Delete(string Id, string Del, string Del1)
        {
            SaleAuth _SaleAuth = new SaleAuth();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST(_SaleAuth, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailDel/" + Id + "/" + Del + "/" + Del1, Getkey());
            return Json(new { success = Call.success });
        }
    }
}