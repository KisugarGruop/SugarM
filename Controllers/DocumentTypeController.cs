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
    public class DocumentTypeController : BaseController<DocumentType> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenId () => _userManager.GetUserId (HttpContext.User);
        public DocumentTypeController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public IActionResult Index () {
            var UserCurrenId = GetCurrenId ();
            var _CompCode = _IUserprofileRepository.GetCompCode (UserCurrenId);
            var year = ConvertYear ();
            List<DocumentType> AuthorList = new List<DocumentType> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocumentTypeGet", Getkey ());
            return View (Call);
        }
        public IActionResult Create () {
            var UserCurrenId = GetCurrenId ();
            var _CompCode = _IUserprofileRepository.GetCompCode (UserCurrenId);
            var year = ConvertYear ();
            var __DocumentType = new DocumentType () {
                CompCode = _CompCode.CompCode,
                CaneYear = year,
            };
            ViewBag.IsEditMode = "false";
            return View (__DocumentType);
        }

        [DisplayName ("เพิ่มหน่วย")]
        [HttpPost]
        public IActionResult Create (DocumentType _DocumentType, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocumentType, "http://192.168.10.46/sdapi/sdapi/DocumentTypePost", Getkey ());
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
                DocumentType _DocumentTypeEdit = new DocumentType () {
                    DocName = _DocumentType.DocName,
                    Description = _DocumentType.Description,
                    Active = _DocumentType.Active,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_DocumentTypeEdit, "http://192.168.10.46/sdapi/sdapi/DocumentTypePut?CompCode=" + _DocumentType.CompCode + "&CaneYear=" + _DocumentType.CaneYear + "&DocCode=" + _DocumentType.DocCode, Getkey ());
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
        public IActionResult Edit (string DocCode, string Id, string CaneYear) {
            ViewBag.IsEditMode = "true";

            List<DocumentType> AuthorList = new List<DocumentType> ();
            DocumentType _DocumentType = new DocumentType ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocumentTypeGet?CompCode=" + Id + "&CaneYear=" + CaneYear + "&DocCode=" + DocCode, Getkey ());
            foreach (var item in Call) {
                _DocumentType.CompCode = item.CompCode;
                _DocumentType.CaneYear = item.CaneYear;
                _DocumentType.DocCode = item.DocCode;
                _DocumentType.DocName = item.DocName;
                _DocumentType.Description = item.Description;
                _DocumentType.Active = item.Active;
            }
            return View ("Create", _DocumentType);
        }

        [DisplayName ("ลบพันธุ์อ้อย")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1) {
            CaneBread _CaneBread = new CaneBread ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneBread, "http://192.168.10.46/sdapi/sdapi/DocumentTypeDel?CompCode=" + Id + "&CaneYear=" + Del + "&DocCode=" + Del1, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}