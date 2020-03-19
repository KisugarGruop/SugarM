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
    public class CarTypeMinorController : BaseController<CarTypeMinor>
    {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser() => _userManager.GetUserName(HttpContext.User);

        public CarTypeMinorController(IClientNotification clientNotification, UserManager<ApplicationUser> userManager)
        {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }

        [DisplayName("เพิ่มยี่ห้อ")]
        public IActionResult CreateCarMinor(string Id, string TypeId, string SubTypeId)
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
            List<CarTypeDetail> AuthorListCarDetail = new List<CarTypeDetail>();
            var _ReCarTypeDetail = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListCarDetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailGet/" + Id + "/" + TypeId + "/" + SubTypeId, Getkey());
            var CarTypeDetail = new List<CarTypeDetail>();
            foreach (var sname in _ReCarTypeDetail)
            {
                CarTypeDetail.Add(new CarTypeDetail
                {
                    SubTypeCode = sname.SubTypeCode,
                    Fullname = sname.SubTypeCode + " | " + sname.Description
                });
            }
            List<CarTypeMinor> AuthorListdetail = new List<CarTypeMinor>();
            CarTypeMinor _CarTypeMinor = new CarTypeMinor();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailDescGet/" + Id + "/" + TypeId + "/" + SubTypeId, Getkey());
            foreach (var item in Call)
            {
                _CarTypeMinor.CompCode = item.CompCode;
                _CarTypeMinor.TypeCode = item.TypeCode;
                _CarTypeMinor.Description = "";
                _CarTypeMinor.Active = item.Active;

            }
            ViewBag.Car = CarType;
            ViewBag.CarDetail = CarTypeDetail;
            _CarTypeMinor.CarTypeMinorlist = Call;
            return View("CreateCarMinor", _CarTypeMinor);
        }

        [DisplayName("แก้ประเภทย่อย")]
        [HttpGet]
        public IActionResult EditDetaill(string Id, string TypeId, string SubId, string MinorId)
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
            List<CarTypeDetail> AuthorListCarDetail = new List<CarTypeDetail>();
            var _ReCarTypeDetail = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListCarDetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailGet/" + Id + "/" + TypeId + "/" + SubId, Getkey());
            var CarTypeDetail = new List<CarTypeDetail>();
            foreach (var sname in _ReCarTypeDetail)
            {
                CarTypeDetail.Add(new CarTypeDetail
                {
                    SubTypeCode = sname.SubTypeCode,
                    Fullname = sname.SubTypeCode + " | " + sname.Description
                });
            }
            List<CarTypeMinor> AuthorListdetail = new List<CarTypeMinor>();
            CarTypeMinor _CarTypeMinor = new CarTypeMinor();
            //* หาค่าที่จะเอามาแก้ไข
            var CarTypeMinordetaill = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/CarTypeMinorGet/" + Id + "/" + TypeId + "/" + SubId + "/" + MinorId, Getkey());
            foreach (var item in CarTypeMinordetaill)
            {
                _CarTypeMinor.CompCode = item.CompCode;
                _CarTypeMinor.TypeCode = item.TypeCode;
                _CarTypeMinor.SubTypeCode = item.SubTypeCode;
                _CarTypeMinor.MinorTypeCode = item.MinorTypeCode;
                _CarTypeMinor.Description = item.Description;
                _CarTypeMinor.WeightIn = item.WeightIn;
                _CarTypeMinor.WeightOut = item.WeightOut;
                _CarTypeMinor.TotalFuel = item.TotalFuel;
                _CarTypeMinor.Active = item.Active;
                _CarTypeMinor.DeleteFlag = item.DeleteFlag;
            }
            List<CarTypeMinor> AuthorListMinordetail = new List<CarTypeMinor>();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorListMinordetail, "http://192.168.10.46/sdapi/sdapi/CarTypeDetailDescGet/" + Id + "/" + TypeId + "/" + SubId, Getkey());
            ViewBag.Car = CarType;
            ViewBag.CarDetail = CarTypeDetail;
            _CarTypeMinor.CarTypeMinorlist = Call;
            return View("CreateCarMinor", _CarTypeMinor);
        }

        [HttpPost]
        [DisplayName("บันทึกประเภทย่อย")]
        public IActionResult SaveCarMinor(CarTypeMinor _CarTypeMinor, string IsEditMode)
        {
            var UserCurrent = GetCurrentUser();
            if (IsEditMode.Equals("false"))
            {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_CarTypeMinor, "http://192.168.10.46/sdapi/sdapi/CarTypeMinorPost", Getkey());
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
                CarTypeMinor _CarMinorUp = new CarTypeMinor()
                {
                    MinorTypeCode = _CarTypeMinor.MinorTypeCode,
                    Description = _CarTypeMinor.Description,
                    WeightIn = _CarTypeMinor.WeightIn,
                    WeightOut = _CarTypeMinor.WeightOut,
                    TotalFuel = _CarTypeMinor.TotalFuel,
                    Active = _CarTypeMinor.Active,
                    DeleteFlag = _CarTypeMinor.DeleteFlag,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime(DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_CarMinorUp, "http://192.168.10.46/sdapi/sdapi/CarTypeMinorPut/" + _CarTypeMinor.CompCode + "/" + _CarTypeMinor.TypeCode + "/" + _CarTypeMinor.SubTypeCode + "/" + _CarTypeMinor.MinorTypeCode, Getkey());
                if (_Re.success)
                {
                    _clientNotification.AddSweetNotification("สำเร็จ",
                        "แก้ไขข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                    return RedirectToAction(nameof(CreateCarMinor), new { Id = _CarTypeMinor.CompCode, TypeId = _CarTypeMinor.TypeCode, SubTypeId = _CarTypeMinor.SubTypeCode });
                }
                else
                {
                    _clientNotification.AddSweetNotification("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                }
            }
            return RedirectToAction(nameof(CreateCarMinor), new { Id = _CarTypeMinor.CompCode, TypeId = _CarTypeMinor.TypeCode, SubTypeId = _CarTypeMinor.SubTypeCode });
        }

        [DisplayName("ลบประเภทย่อย")]
        [HttpGet]
        public IActionResult Delete(string Id, string Del, string Del1, string Del2)
        {
            SaleAuth _SaleAuth = new SaleAuth();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST(_SaleAuth, "http://192.168.10.46/sdapi/sdapi/CarTypeMinorDel/" + Id + "/" + Del + "/" + Del1 + "/" + Del2, Getkey());
            return Json(new { success = Call.success });
        }
    }
}