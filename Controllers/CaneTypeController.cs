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
    public class CaneTypeController : BaseController<CaneType> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public CaneTypeController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            var _CompCode = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var year = ConvertYear ();
            List<CaneType> AuthorList = new List<CaneType> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/CaneTypeGet", Getkey ());
            return View (Call);
        }
        public IActionResult Create () {
            var CaneTypeemodel = new CaneType ();
            ViewBag.IsEditMode = "false";
            return View (CaneTypeemodel);
        }

        [DisplayName ("บันทึกประเภทอ้อย")]
        [HttpPost]
        public async Task<IActionResult> Create (CaneType _CaneType, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneType, "http://192.168.10.46/sdapi/sdapi/CaneTypePost", Getkey ());
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
                    CaneType _CaneTypeDetail = new CaneType () {
                        TypeCode = _CaneType.TypeCode,
                        Description = _CaneType.Description,
                        Pass = _CaneType.Pass,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneTypeDetail, "http://192.168.10.46/sdapi/sdapi/CaneTypePut/" + _CaneType.TypeCode, Getkey ());
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
            return View ("Create", _CaneType);
        }

        [DisplayName ("แก้ไขประเภทอ้อย")]
        [HttpGet]
        public IActionResult Edit (string Id) {
            ViewBag.IsEditMode = "true";

            List<CaneType> AuthorList = new List<CaneType> ();
            CaneType _CaneType = new CaneType ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/CaneTypeGet/" + Id, Getkey ());
            foreach (var item in Call) {
                _CaneType.TypeCode = item.TypeCode;
                _CaneType.Description = item.Description;
                _CaneType.Pass = item.Pass;
            }
            return View ("Create", _CaneType);
        }

        [DisplayName ("ลบประเภทอ้อย")]
        [HttpGet]
        public IActionResult Delete (string Id) {
            SaleAuth _SaleAuth = new SaleAuth ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_SaleAuth, "http://192.168.10.46/sdapi/sdapi/CaneTypeDel/" + Id, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}