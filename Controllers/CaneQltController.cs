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
    public class CaneQltController : BaseController<CaneQlt> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public CaneQltController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            var _CompCode = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var year = ConvertYear ();
            List<CaneQlt> AuthorList = new List<CaneQlt> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/CaneQltGet?CompCode=" + _CompCode.CompCode + "&CaneYear=" + year, Getkey ());
            return View (Call);
        }

        public async Task<IActionResult> Create () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            string Year = ConvertYear ();
            var _CompCode = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var CaneQltmodel = new CaneQlt () {
                CompCode = _CompCode.CompCode,
                CaneYear = Year,
            };
            ViewBag.IsEditMode = "false";
            return View (CaneQltmodel);
        }

        [DisplayName ("บันทึกคุณภาพอ้อย")]
        [HttpPost]
        public async Task<IActionResult> Create (CaneQlt _CaneQlt, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneQlt, "http://192.168.10.46/sdapi/sdapi/CaneQltPost", Getkey ());
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
                    CaneQlt _CaneQltDetail = new CaneQlt () {
                        CompCode = _CaneQlt.CompCode,
                        CaneYear = _CaneQlt.CaneYear,
                        Quality = _CaneQlt.Quality,
                        Description = _CaneQlt.Description,
                        Pass = _CaneQlt.Pass,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneQltDetail, "http://192.168.10.46/sdapi/sdapi/CaneQltPut/" + _CaneQlt.CompCode + "/" + _CaneQlt.CaneYear + "/" + _CaneQlt.Quality, Getkey ());
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
            return View ("Create", _CaneQlt);
        }

        [DisplayName ("แก้คุณภาพอ้อย")]
        [HttpGet]
        public IActionResult Edit (string Id, string CaneYear, string Quality) {
            ViewBag.IsEditMode = "true";

            List<CaneQlt> AuthorList = new List<CaneQlt> ();
            CaneQlt _CaneQlt = new CaneQlt ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/CaneQltGet?Compcode=" + Id + "&CaneYear=" + CaneYear + "&Quality=" + Quality, Getkey ());
            foreach (var item in Call) {
                _CaneQlt.CompCode = item.CompCode;
                _CaneQlt.CaneYear = item.CaneYear;
                _CaneQlt.Quality = item.Quality;
                _CaneQlt.Description = item.Description;
                _CaneQlt.Pass = item.Pass;
            }
            return View ("Create", _CaneQlt);
        }

        [DisplayName ("ลบคุณภาพอ้อย")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1) {
            CaneQlt _CaneQlt = new CaneQlt ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneQlt, "http://192.168.10.46/sdapi/sdapi/CaneQltDel?CompCode=" + Id + "&CaneYear=" + Del + "&Quality=" + Del1, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}