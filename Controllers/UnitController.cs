using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.Repository;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class UnitController : BaseController<Unit> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenId () => _userManager.GetUserId (HttpContext.User);
        public UnitController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }

        public IActionResult Index () {
            var UserCurrenId = GetCurrenId ();
            var _CompCode = _IUserprofileRepository.GetCompCode (UserCurrenId);
            var year = ConvertYear ();
            List<Unit> AuthorList = new List<Unit> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/UnitGet", Getkey ());
            return View (Call);
        }
        public IActionResult Create () {
            var UserCurrenId = GetCurrenId ();
            var _CompCode = _IUserprofileRepository.GetCompCode (UserCurrenId);
            var _Unit = new Unit () {
                CompCode = _CompCode.CompCode,
            };
            ViewBag.IsEditMode = "false";
            return View (_Unit);
        }

        [DisplayName ("เพิ่มหน่วย")]
        [HttpPost]
        public IActionResult Create (Unit _Unit, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_Unit, "http://192.168.10.46/sdapi/sdapi/UnitPost", Getkey ());
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
                Unit _UnitEdit = new Unit () {
                    UnitName = _Unit.UnitName,
                    Description = _Unit.Description,
                    Active = _Unit.Active,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_UnitEdit, "http://192.168.10.46/sdapi/sdapi/UnitPut/" + _Unit.CompCode + "/" + _Unit.UnitName, Getkey ());
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

        [DisplayName ("แก้พันธุ์อ้อย")]
        [HttpGet]
        public IActionResult Edit (string Id, string CompCode) {
            ViewBag.IsEditMode = "true";

            List<Unit> AuthorList = new List<Unit> ();
            Unit _Unit = new Unit ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/UnitGet/" + CompCode + "/" + Id, Getkey ());
            foreach (var item in Call) {
                _Unit.CompCode = item.CompCode;
                _Unit.UnitName = item.UnitName;
                _Unit.Description = item.Description;
                _Unit.Active = item.Active;
            }
            return View ("Create", _Unit);
        }

        [DisplayName ("ลบพันธุ์อ้อย")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del) {
            CaneBread _CaneBread = new CaneBread ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneBread, "http://192.168.10.46/sdapi/sdapi/UnitDel/" + Id + "/" + Del, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}