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
    public class UserLevelController : BaseController<UserLevel> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public UserLevelController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            List<UserLevel> AuthorList = new List<UserLevel> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/UserIdLevelNGet/" + _UserProfile.CompCode, Getkey ());
            return View (Call);
        }
        public async Task<IActionResult> Create () {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            var Year = ConvertYear ();
            var UserLevelmodel = new UserLevel () {
                CompCode = _UserProfile.CompCode,
                CaneYear = Year

            };

            //*Dropdown UserSugarM ---------------------------
            List<UserSugarM> AuthorList = new List<UserSugarM> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/UserLike/" + _UserProfile.CompCode, Getkey ());
            var Userfull = new List<UserSugarM> ();
            foreach (var sname in Call) {
                Userfull.Add (new UserSugarM {
                    UserId = sname.UserId,
                        UserFullname = sname.UserId + " " + sname.UserName
                });
            }

            ViewBag.UserSugarM = Userfull;
            ViewBag.IsEditMode = "false";
            return View (UserLevelmodel);
        }

        [DisplayName ("เพิ่มระดับผู้ใช้งาน")]
        [HttpPost]
        public async Task<IActionResult> Create (UserLevel _UserLevel, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_UserLevel, "http://192.168.10.46/sdapi/sdapi/UserIdLevelPost", Getkey ());
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
                UserLevel _UserLevelEdit = new UserLevel () {
                    CompCode = _UserLevel.CompCode,
                    CaneYear = _UserLevel.CaneYear,
                    UserId = _UserLevel.UserId,
                    UseridLevel = _UserLevel.UseridLevel,
                    Description = _UserLevel.Description,
                    Active = _UserLevel.Active,
                    UpdateBy = _UserProfile.EmployeeId,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_UserLevelEdit, "http://192.168.10.46/sdapi/sdapi/UserIdLevelPut?CompCode=" + _UserLevel.CompCode + "&CaneYear=" + _UserLevel.CaneYear + "&UserId=" + _UserLevel.UserId, Getkey ());
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

        [DisplayName ("แก้ระดับผู้ใช้งาน")]
        [HttpGet]
        public IActionResult Edit (string CompCode, string CaneYear, string UserId) {
            ViewBag.IsEditMode = "true";

            List<UserLevel> AuthorList = new List<UserLevel> ();
            UserLevel _UserLevel = new UserLevel ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/UserIdOverrideLevelGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&UserId=" + UserId, Getkey ());
            foreach (var item in Call) {
                _UserLevel.CompCode = item.CompCode;
                _UserLevel.CaneYear = item.CaneYear;
                _UserLevel.UserId = item.UserId;
                _UserLevel.UseridLevel = item.UseridLevel;
                _UserLevel.Description = item.Description;
                _UserLevel.MinAmount = item.MinAmount;
                _UserLevel.MaxAmount = item.MaxAmount;
                _UserLevel.Active = item.Active;
            }

            //*Dropdown UserSugarM ---------------------------
            List<UserSugarM> _DrowAuthorList = new List<UserSugarM> ();
            var CallDrow = ServiceExtension.RestshapExtension.CallRestApiGET (_DrowAuthorList, "http://192.168.10.46/sdapi/sdapi/UserLike/" + CompCode, Getkey ());
            foreach (var sname in CallDrow) {
                _UserLevel.Fullname = sname.UserId + " " + sname.UserName;
            }
            return View ("Create", _UserLevel);
        }
        public async Task<IActionResult> GetOverride () {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            List<Override> AuthorList = new List<Override> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/OverrideLevelGet?CompCode=" + _UserProfile.CompCode + "&CaneYear=" + ConvertYear (), Getkey ());
            return PartialView ("_ModalCMPartial", Call);
        }

    }
}