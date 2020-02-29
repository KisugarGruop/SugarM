using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClientNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SugarM.Models;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    [Authorize]
    [DisplayName ("Bank Manage")]
    public class BankController : BaseController<Bank> {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);

        public BankController (IClientNotification clientNotification, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }

        public IActionResult Index (int page = 1) {
            List<Bank> AuthorList = new List<Bank> ();
            var Call = CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/bankGet");
            var paginatedResult = PaginatedResult (Call, page, 10);
            return View (paginatedResult);
        }

        [DisplayName ("ดูรายละเอียดสาขาแบงค์")]
        public IActionResult GetBankBranch (string Id, int page) {
            List<Bank> AuthorList = new List<Bank> ();
            var Call = CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/bankBranchGet/" + Id);
            var paginatedResult = PaginatedResult (Call, page, 10);
            ViewBag.BankCode = Id;
            return View (paginatedResult);
        }

        public IActionResult Create () {
            var Bankmodel = new Bank ();
            ViewBag.IsEditMode = "false";
            return View (Bankmodel);
        }

        [DisplayName ("เพิ่มแบงค์")]
        [HttpPost]
        public IActionResult Create (Bank Bankmodel, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = CallRestApiPOST (Bankmodel, "http://192.168.10.46/sdapi/sdapi/bankPost");
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
                Bank bankup = new Bank () {
                    BankCode = Bankmodel.BankCode,
                    BankName = Bankmodel.BankName,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = CallRestApiPOST (bankup, "http://192.168.10.46/sdapi/sdapi/bankPut/" + Bankmodel.BankCode);
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

        [Route ("Bank/Createbankbranch/{Idbranch}")]
        public IActionResult Createbankbranch (string Idbranch) {
            ViewBag.IsEditMode = "false";
            List<Bank> AuthorList = new List<Bank> ();
            Bank Bankmodel = new Bank ();
            var Call = CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/bankGet/" + Idbranch);
            foreach (var item in Call) {
                Bankmodel.BankCode = item.BankCode;
                Bankmodel.BankName = item.BankName;
            }
            return View (Bankmodel);
        }

        [DisplayName ("เพิ่มสาขาแบงค์")]
        [HttpPost]
        public IActionResult CreatebankbranchNew (Bank Bankmodel, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = CallRestApiPOST (Bankmodel, "http://192.168.10.46/sdapi/sdapi/bankBranchPost");
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
                Bank bankup = new Bank () {
                    BankCode = Bankmodel.BankCode,
                    BankName = Bankmodel.BankName,
                    BranchCode = Bankmodel.BranchCode,
                    BranchName = Bankmodel.BranchName,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = CallRestApiPOST (bankup, "http://192.168.10.46/sdapi/sdapi/bankBranchPut/" + Bankmodel.BankCode + "/" + Bankmodel.BranchCode);
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

        [DisplayName ("ลบแบงค์")]
        [HttpGet]
        public IActionResult Delete (string Id) {
            Bank Bankmodel = new Bank ();
            var Call = CallRestApiPOST (Bankmodel, "http://192.168.10.46/sdapi/sdapi/bankDel/" + Id);
            return Json (new { success = Call.success });
        }

        [DisplayName ("ลบสาขาแบงค์")]
        [HttpGet]
        public IActionResult DeleteBranch (string Id, string Del) {
            Bank Bankmodel = new Bank ();
            var Call = CallRestApiPOST (Bankmodel, "http://192.168.10.46/sdapi/sdapi/bankBranchDel/" + Id + "/" + Del);
            return Json (new { success = Call.success });
        }

        [DisplayName ("แก้ไขแบงค์")]
        [HttpGet]
        public IActionResult Edit (string Id) {
            ViewBag.IsEditMode = "true";
            List<Bank> AuthorList = new List<Bank> ();
            Bank Bankmodel = new Bank ();
            var Call = CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/bankGet/" + Id);
            foreach (var item in Call) {
                Bankmodel.BankCode = item.BankCode;
                Bankmodel.BankName = item.BankName;
            }
            return View ("Create", Bankmodel);
        }

        [DisplayName ("แก้ไขสาขาแบงค์")]
        [HttpGet]
        public IActionResult EditBankBranch (string Id, string branchId) {
            ViewBag.IsEditMode = "true";
            List<Bank> AuthorList = new List<Bank> ();
            Bank Bankmodel = new Bank ();
            var Call = CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/BankBankBranchGet/" + Id + "/" + branchId);
            foreach (var item in Call) {
                Bankmodel.BankCode = item.BankCode;
                Bankmodel.BankName = item.BankName;
                Bankmodel.BranchCode = item.BranchCode;
                Bankmodel.BranchName = item.BranchName;
            }
            return View ("Createbankbranch", Bankmodel);
        }
    }
}