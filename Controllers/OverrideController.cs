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
    public class OverrideController : BaseController<Override> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public OverrideController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCompCode = GetCurrenCompCode ();
            var _CompCode = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            List<Override> AuthorList = new List<Override> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/OverrideLevelGet/" + _CompCode.CompCode, Getkey ());
            return View (Call);
        }

        public async Task<IActionResult> Create () {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            var Year = ConvertYear ();
            var Overridemodel = new Override () {
                CompCode = _UserProfile.CompCode,
                CaneYear = Year

            };
            ViewBag.IsEditMode = "false";
            return View (Overridemodel);
        }

        [DisplayName ("เพิ่มระดับผู้อนุมัติ")]
        [HttpPost]
        public async Task<IActionResult> Create (Override _Override, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_Override, "http://192.168.10.46/sdapi/sdapi/OverrideLevelPost", Getkey ());
                    if (_Re.success) {
                        _clientNotification.AddSweetNotification ("สำเร็จ",
                            "บันทึกข้อมูลเรียบร้อยแล้ว",
                            NotificationHelper.NotificationType.success);
                    } else {
                        _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                            _Re.message,
                            NotificationHelper.NotificationType.error);
                    }
                } else {
                    Override _OverrideEdit = new Override () {
                        CompCode = _Override.CompCode,
                        CaneYear = _Override.CaneYear,
                        OverrideLevel = _Override.OverrideLevel,
                        MinAmount = _Override.MinAmount,
                        MaxAmount = _Override.MaxAmount,
                        DeleteFlag = _Override.DeleteFlag,
                        Description = _Override.Description,
                        Active = _Override.Active,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_OverrideEdit, "http://192.168.10.46/sdapi/sdapi/OverrideLevelPut?CompCode=" + _Override.CompCode + "&CaneYear=" + _Override.CaneYear + "&OverrideLevel=" + _Override.OverrideLevel, Getkey ());
                    if (_Re.success) {
                        _clientNotification.AddSweetNotification ("สำเร็จ",
                            "แก้ไขข้อมูลเรียบร้อยแล้ว",
                            NotificationHelper.NotificationType.success);
                    } else {
                        _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                            _Re.message,
                            NotificationHelper.NotificationType.error);
                    }
                }
                return RedirectToAction (nameof (Index));
            }
            return View ("Create", _Override);
        }

        [DisplayName ("แก้ไขผู้อนุมัติ")]
        [HttpGet]
        public IActionResult Edit (string Compcode, string CaneYear, string OverrideLevel) {
            ViewBag.IsEditMode = "true";

            List<Override> AuthorList = new List<Override> ();
            Override _Override = new Override ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/OverrideLevelGet?CompCode=" + Compcode + "&CaneYear=" + CaneYear + "&GradeCode=" + OverrideLevel, Getkey ());
            foreach (var item in Call) {
                _Override.CompCode = item.CompCode;
                _Override.CaneYear = item.CaneYear;
                _Override.OverrideLevel = item.OverrideLevel;
                _Override.MaxAmount = item.MaxAmount;
                _Override.MinAmount = item.MinAmount;
                _Override.Description = item.Description;
                _Override.Active = item.Active;
            }
            return View ("Create", _Override);
        }

        [DisplayName ("ลบเกรดจัดชั้นหนี้")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1) {
            Override _Override = new Override ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_Override, "http://192.168.10.46/sdapi/sdapi/OverrideLevelDel?CompCode=" + Id + "&CaneYear=" + Del + "&OverrideLevel=" + Del1, Getkey ());
            return Json (new { success = Call.success });
        }

    }
}