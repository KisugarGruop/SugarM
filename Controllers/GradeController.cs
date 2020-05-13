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
    public class GradeController : BaseController<Grade> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public GradeController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            List<Grade> AuthorList = new List<Grade> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/GradeGet?CompCode=" + _UserProfile.CompCode + "&CaneYear=" + ConvertYear (), Getkey ());
            return View (Call);
        }
        public async Task<IActionResult> Create () {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            var Year = ConvertYear ();
            var Grademodel = new Grade () {
                CompCode = _UserProfile.CompCode,
                CaneYear = Year

            };
            ViewBag.IsEditMode = "false";
            return View (Grademodel);
        }

        [DisplayName ("แก้ไขเกรดจัดชั้นหนี้")]
        [HttpGet]
        public IActionResult Edit (string CaneYear, string cmid, string GradeCode) {
            ViewBag.IsEditMode = "true";

            List<Grade> AuthorList = new List<Grade> ();
            Grade _grade = new Grade ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/GradeGet?CompCode=" + cmid + "&CaneYear=" + CaneYear + "&GradeCode=" + GradeCode, Getkey ());
            foreach (var item in Call) {
                _grade.CompCode = item.CompCode;
                _grade.CaneYear = item.CaneYear;
                _grade.GradeCode = item.GradeCode;
                _grade.Description = item.Description;
                _grade.Active = item.Active;
            }
            return View ("Create", _grade);
        }

        [DisplayName ("บันทึกเกรดจัดชั้นหนี้")]
        [HttpPost]
        public async Task<IActionResult> Create (Grade _Grade, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_Grade, "http://192.168.10.46/sdapi/sdapi/GradePost", Getkey ());
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
                    Grade _GradeDetaill = new Grade () {
                        CompCode = _Grade.CompCode,
                        CaneYear = _Grade.CaneYear,
                        GradeCode = _Grade.GradeCode,
                        Description = _Grade.Description,
                        Active = _Grade.Active,
                        DeleteFlag = _Grade.DeleteFlag,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_GradeDetaill, "http://192.168.10.46/sdapi/sdapi/GradePut?CompCode=" + _Grade.CompCode + "&CaneYear=" + _Grade.CaneYear + "&GradeCode=" + _Grade.GradeCode, Getkey ());
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
            return View ("Create", _Grade);
        }

        [DisplayName ("ลบเกรดจัดชั้นหนี้")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1) {
            Grade _Grade = new Grade ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_Grade, "http://192.168.10.46/sdapi/sdapi/GradeDel?CompCode=" + Id + "&CaneYear=" + Del + "&GradeCode=" + Del1, Getkey ());
            return Json (new { success = Call.success });
        }

    }
}