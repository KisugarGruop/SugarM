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
    public class CaneBreadController : BaseController<CarBrand> {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public CaneBreadController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }

        public IActionResult Index () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            var _CompCode = _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var year = ConvertYear ();
            List<CaneBread> AuthorList = new List<CaneBread> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/CaneBreadGet", Getkey ());
            return View (Call);
        }
        public IActionResult Create () {
            var _CaneBread = new CaneBread ();
            ViewBag.IsEditMode = "false";
            return View (_CaneBread);
        }

        [DisplayName ("เพิ่มพันธุ์อ้อย")]
        [HttpPost]
        public async Task<IActionResult> Create (CaneBread _CaneBread, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneBread, "http://192.168.10.46/sdapi/sdapi/CaneBreadPost", Getkey ());
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
                CaneBread _CaneEdit = new CaneBread () {
                    BreadName = _CaneBread.BreadName,
                    BreadWeight = _CaneBread.BreadWeight,
                    remarks = _CaneBread.remarks,
                    UpdateBy = _UserProfile.EmployeeId,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneEdit, "http://192.168.10.46/sdapi/sdapi/CaneBreadPut/" + _CaneBread.BreadName, Getkey ());
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
        public IActionResult Edit (string Id) {
            ViewBag.IsEditMode = "true";

            List<CaneBread> AuthorList = new List<CaneBread> ();
            CaneBread _CaneBread = new CaneBread ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/CaneBreadGet/" + Id, Getkey ());
            foreach (var item in Call) {
                _CaneBread.BreadName = item.BreadName;
                _CaneBread.BreadWeight = item.BreadWeight;
                _CaneBread.remarks = item.remarks;
            }
            return View ("Create", _CaneBread);
        }

        [DisplayName ("ลบพันธุ์อ้อย")]
        [HttpGet]
        public IActionResult Delete (string Id) {
            CaneBread _CaneBread = new CaneBread ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_CaneBread, "http://192.168.10.46/sdapi/sdapi/CaneBreadDel/" + Id, Getkey ());
            return Json (new { success = Call.success });
        }

    }
}