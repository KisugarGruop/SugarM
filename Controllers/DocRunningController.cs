using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.Repository;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class DocRunningController : BaseController<DocRunning> {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public DocRunningController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public IActionResult Index () {
            List<DocRunning> AuthorList = new List<DocRunning> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocRunningGet", Getkey ());
            return View (Call);
        }

        [DisplayName ("แก้ไขเอกสาร")]
        [HttpGet]
        public IActionResult Edit (string Id, string cmid) {
            ViewBag.IsEditMode = "true";

            List<DocRunning> AuthorList = new List<DocRunning> ();
            DocRunning _DocRunning = new DocRunning ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocRunningGet/" + cmid + "/" + Id, Getkey ());
            foreach (var item in Call) {
                _DocRunning.CompCode = item.CompCode;
                _DocRunning.RunningId = item.RunningId;
                _DocRunning.RunningName = item.RunningName;
                _DocRunning.RunningMark = item.RunningMark;
                _DocRunning.RunningYear = item.RunningYear;
                _DocRunning.SeparateChar = item.SeparateChar;
                _DocRunning.DigitRunning = item.DigitRunning;
                _DocRunning.LastRunning = item.LastRunning;
            }
            return View ("Create", _DocRunning);
        }

        public async Task<IActionResult> Create () {

            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            var DocRunningmodel = new DocRunning () {
                CompCode = _UserProfile.CompCode,
            };

            ViewBag.IsEditMode = "false";
            return View (DocRunningmodel);
        }

        [DisplayName ("ลบเอกสาร")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del) {
            DocRunning _DocRunning = new DocRunning ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocRunning, "http://192.168.10.46/sdapi/sdapi/DocRunningDel/" + Id + "/" + Del, Getkey ());
            return Json (new { success = Call.success });
        }

        [DisplayName ("บันทึกประเภทรถ")]
        [HttpPost]
        public async Task<IActionResult> Create (DocRunning _DocRunning, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            DateTime FristYear = DateTime.Today;
            string BasedFrist = FristYear.ToString ("yyyy", new CultureInfo ("th-TH"));
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    _DocRunning.CompCode = _UserProfile.CompCode;
                    _DocRunning.RunningYear = BasedFrist;
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocRunning, "http://192.168.10.46/sdapi/sdapi/DocRunningPost", Getkey ());
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
                    DocRunning _DocRunningDetaill = new DocRunning () {
                        CompCode = _DocRunning.CompCode,
                        RunningId = _DocRunning.RunningId,
                        RunningName = _DocRunning.RunningName,
                        RunningMark = _DocRunning.RunningMark,
                        RunningYear = _DocRunning.RunningYear,
                        SeparateChar = _DocRunning.SeparateChar,
                        DigitRunning = _DocRunning.DigitRunning,
                        LastRunning = _DocRunning.LastRunning,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocRunningDetaill, "http://192.168.10.46/sdapi/sdapi/DocRunningPut/" + _DocRunning.CompCode + "/" + _DocRunning.RunningId, Getkey ());
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
            return View ("Create", _DocRunning);
        }
    }
}