using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.Repository;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class CarTypeController : BaseController<CarType> {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);

        public CarTypeController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }

        public IActionResult Index () {
            List<CarType> AuthorList = new List<CarType> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/CarTypeGet", Getkey ());
            return View (Call);
        }
        public IActionResult Create () {
            var Cartypemodel = new CarType ();
            ViewBag.IsEditMode = "false";
            return View (Cartypemodel);
        }

        [DisplayName ("บันทึกประเภทรถ")]
        [HttpPost]
        public async Task<IActionResult> Create (CarType _Cartype, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_Cartype, "http://192.168.10.46/sdapi/sdapi/CarTypePost", Getkey ());
                    if (_Re.success) {
                        _clientNotification.AddSweetNotification ("สำเร็จ",
                            "บันทึกข้อมูลเรียบร้อยแล้ว",
                            NotificationHelper.NotificationType.success);
                    } else {
                        _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                            _Re.message,
                            NotificationHelper.NotificationType.error);
                        return RedirectToAction (nameof (Index));
                    }
                } else {
                    CarType _CarTypeDetail = new CarType () {
                        CompCode = _Cartype.CompCode,
                        TypeCode = _Cartype.TypeCode,
                        Description = _Cartype.Description,
                        Active = _Cartype.Active,
                        DeleteFlag = _Cartype.DeleteFlag,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CarTypeDetail, "http://192.168.10.46/sdapi/sdapi/CarTypePut/" + _Cartype.CompCode + "/" + _Cartype.TypeCode, Getkey ());
                    if (_Re.success) {
                        _clientNotification.AddSweetNotification ("สำเร็จ",
                            "แก้ไขข้อมูลเรียบร้อยแล้ว",
                            NotificationHelper.NotificationType.success);
                    } else {
                        _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                            _Re.message,
                            NotificationHelper.NotificationType.error);
                        return RedirectToAction (nameof (Index));
                    }
                }
                return RedirectToAction (nameof (Index));
            }
            return View ("Create", _Cartype);
        }

        [DisplayName ("แก้ประเภทรถ")]
        [HttpGet]
        public IActionResult Edit (string Id, string TypeId) {
            ViewBag.IsEditMode = "true";

            List<CarType> AuthorList = new List<CarType> ();
            CarType _CarType = new CarType ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/CarTypeGet/" + Id + "/" + TypeId, Getkey ());
            foreach (var item in Call) {
                _CarType.CompCode = item.CompCode;
                _CarType.TypeCode = item.TypeCode;
                _CarType.Description = item.Description;
                _CarType.Active = item.Active;
            }
            return View ("Create", _CarType);
        }

        [DisplayName ("ลบประเภทรถ")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del) {
            SaleAuth _SaleAuth = new SaleAuth ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_SaleAuth, "http://192.168.10.46/sdapi/sdapi/CarTypeDel/" + Id + "/" + Del, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}