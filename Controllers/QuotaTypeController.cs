using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class QuotaTypeController : BaseController<QuotaType> {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);

        public QuotaTypeController (IClientNotification clientNotification, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }

        public IActionResult Index (int page = 1) {
            List<QuotaType> AuthorList = new List<QuotaType> ();
            var Call = CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/QuotaTypeGet");
            return View (Call);
        }
        public IActionResult Create () {
            var _Quotamodel = new QuotaViewModel ();
            ViewBag.IsEditMode = "false";
            return View (_Quotamodel);
        }

        [DisplayName ("เพิ่มประเภทโควต้า")]
        [HttpPost]
        public IActionResult Create (QuotaViewModel _Quotamodel, string IsEditMode) {
            //*-------- Sen to save api
            QuotaType _Quotagetapi = new QuotaType () {
                TypeCode = _Quotamodel.TypeCode,
                Description = _Quotamodel.Description
            };

            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = CallRestApiPOST (_Quotagetapi, "http://192.168.10.46/sdapi/sdapi/QuotaTypePost");
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
                QuotaType _Quota = new QuotaType () {
                    TypeCode = _Quotamodel.TypeCode,
                    Description = _Quotamodel.Description,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = CallRestApiPOST (_Quota, "http://192.168.10.46/sdapi/sdapi/QuotaTypePut/" + _Quota.TypeCode);
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

        [DisplayName ("ลบประเภทโควต้า")]
        [HttpGet]
        public IActionResult Delete (string Id) {
            QuotaType _Quota = new QuotaType ();
            var Call = CallRestApiPOST (_Quota, "http://192.168.10.46/sdapi/sdapi/QuotaTypeDel/" + Id);
            return Json (new { success = Call.success });
        }

        [DisplayName ("แก้ประเภทโควต้า")]
        [HttpGet]
        public IActionResult Edit (string Id) {
            ViewBag.IsEditMode = "true";
            List<QuotaType> AuthorList = new List<QuotaType> ();
            QuotaViewModel _Quota = new QuotaViewModel ();
            var Call = CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/QuotaTypeGet/" + Id);
            foreach (var item in Call) {
                _Quota.TypeCode = item.TypeCode;
                _Quota.Description = item.Description;
            }
            return View ("Create", _Quota);
        }
    }
}