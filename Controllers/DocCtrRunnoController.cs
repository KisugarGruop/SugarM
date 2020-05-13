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
    public class DocCtrRunnoController : BaseController<DocCtrRunno> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public DocCtrRunnoController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var year = ConvertYear ();
            List<DocCtrRunno> AuthorList = new List<DocCtrRunno> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocCtrlRunnoNGet?Compcode=" + _UserProfile.CompCode + "&CaneYear=" + year, Getkey ());
            return View (Call);
        }

        [DisplayName ("แก้ไขเอกสาร")]
        [HttpGet]
        public IActionResult Edit (string cmid, string Caneyear, string Doccode, string DocType, string BranchSy) {
            ViewBag.IsEditMode = "true";

            List<DocCtrRunno> AuthorList = new List<DocCtrRunno> ();
            DocCtrRunno _DocCtrrunnomodel = new DocCtrRunno ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocCtrlRunnoGet?CompCode=" + cmid + "&CaneYear=" + Caneyear + "&DocCode=" + Doccode + "&DocType=" + DocType + "&BranchSymbol=" + BranchSy, Getkey ());
            foreach (var item in Call) {
                _DocCtrrunnomodel.CompCode = item.CompCode;
                _DocCtrrunnomodel.CaneYear = item.CaneYear;
                _DocCtrrunnomodel.DocCode = item.DocCode;
                _DocCtrrunnomodel.DocType = item.DocType;
                _DocCtrrunnomodel.BranchCode = item.BranchCode;
                _DocCtrrunnomodel.BranchSymbol = item.BranchSymbol;
                _DocCtrrunnomodel.LastRunNo = item.LastRunNo;
                _DocCtrrunnomodel.DigitRunNo = item.DigitRunNo;
                _DocCtrrunnomodel.CodeName = item.CodeName;
                _DocCtrrunnomodel.TypeName = item.TypeName;
            }
            //*Dropdown sale ---------------------------
            List<Companysale> Companyget1 = new List<Companysale> ();
            var _rec2 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/branchGet", Getkey ());
            var branch = new List<Companysale> ();
            foreach (var users in _rec2) {
                branch.Add (new Companysale {
                    BranchCode = users.BranchCode,
                        DpComIdandname = users.BranchCode + " " + users.NameTH,

                });

            }
            ViewBag.branch = branch;
            return View ("Create", _DocCtrrunnomodel);
        }

        public async Task<IActionResult> Create () {

            var UserCurrenCompCode = GetCurrenCompCode ();
            string Year = ConvertYear ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var DocCtrrunnomodel = new DocCtrRunno () {
                CompCode = _UserProfile.CompCode,
                CaneYear = Year,
                BranchSymbol = "00",
            };
            //*Dropdown sale ---------------------------
            List<Companysale> Companyget1 = new List<Companysale> ();
            var _rec2 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/branchGet", Getkey ());
            var branch = new List<Companysale> ();
            foreach (var users in _rec2) {
                branch.Add (new Companysale {
                    BranchCode = users.BranchCode,
                        DpComIdandname = users.BranchCode + " " + users.NameTH,

                });

            }
            ViewBag.branch = branch;
            ViewBag.IsEditMode = "false";
            return View (DocCtrrunnomodel);
        }

        [DisplayName ("ลบเอกสาร")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1, string Del2, string Del3) {
            DocCtrRunno _DocRunning = new DocCtrRunno ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocRunning, "http://192.168.10.46/sdapi/sdapi/DocCtrlRunnoDel?CompCode=" + Id + "&CaneYear=" + Del + "&DocCode=" + Del1 + "&DocType=" + Del2 + "&BranchSymbol=" + Del3, Getkey ());
            return Json (new { success = Call.success });
        }

        [DisplayName ("บันทึกประเภทรถ")]
        [HttpPost]
        public async Task<IActionResult> Create (DocCtrRunno _DocRunning, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocRunning, "http://192.168.10.46/sdapi/sdapi/DocCtrlRunnoPost", Getkey ());
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
                    DocCtrRunno _DocRunningDetaill = new DocCtrRunno () {
                        CompCode = _DocRunning.CompCode,
                        CaneYear = _DocRunning.CaneYear,
                        DocCode = _DocRunning.DocCode,
                        DocType = _DocRunning.DocType,
                        BranchSymbol = _DocRunning.BranchSymbol,
                        LastRunNo = _DocRunning.LastRunNo,
                        DigitRunNo = _DocRunning.DigitRunNo,
                        CodeName = _DocRunning.CodeName,
                        TypeName = _DocRunning.TypeName,
                        BranchCode = _DocRunning.BranchCode,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocRunningDetaill, "http://192.168.10.46/sdapi/sdapi/DocCtrlRunnoPut?CompCode=" + _DocRunning.CompCode + "&CaneYear=" + _DocRunning.CaneYear + "&DocCode=" + _DocRunning.DocCode + "&DocType=" + _DocRunning.DocType + "&BranchSymbol=" + _DocRunning.BranchSymbol, Getkey ());
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
            return View ("Create", _DocRunning);
        }
    }
}