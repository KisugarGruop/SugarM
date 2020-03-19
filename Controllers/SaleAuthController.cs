using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class SaleAuthController : BaseController<SaleAuth> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);

        public SaleAuthController (IClientNotification clientNotification, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }
        public IActionResult Index () {
            List<SaleAuth> AuthorList = new List<SaleAuth> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/SaleAuthget", Getkey ());

            return View (Call);
        }
        public IActionResult Create (string _Id) {
            //*Dropdown sale ---------------------------
            List<SaleAuth> AuthorList = new List<SaleAuth> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet", Getkey ());
            var salefull = new List<SaleAuth> ();
            foreach (var sname in Call) {
                salefull.Add (new SaleAuth {
                    SaleId = sname.SaleId,
                        SaleFullname = sname.SaleId + " " + sname.SaleName
                });
            }
            List<SaleAuth> Companyget1 = new List<SaleAuth> ();
            var Call2 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/regionGet", Getkey ());
            var branch = new List<SaleAuth> ();
            foreach (var users in Call2) {
                branch.Add (new SaleAuth {
                    RegionCode = users.RegionCode,
                        RegCodeAndName = users.RegionCode + " " + users.NameTH
                });

            }
            List<SaleAuth> saleview = new List<SaleAuth> ();
            SaleAuthViewmodel _Salestamodel = new SaleAuthViewmodel ();
            if (_Id == null) {

            } else {
                var saleviewmodel = ServiceExtension.RestshapExtension.CallRestApiGET (saleview, "http://192.168.10.46/sdapi/sdapi/saleAuthget/" + _Id, Getkey ());
                _Salestamodel.Salelist = saleviewmodel;
            }
            ViewBag.sale = salefull;
            ViewBag.branch = branch;

            ViewBag.IsEditMode = "false";
            return View (_Salestamodel);
        }

        [DisplayName ("เพิ่มประเภทโควต้า")]
        [HttpPost]
        public IActionResult Create (SaleAuth _SaleAuth, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_SaleAuth, "http://192.168.10.46/sdapi/sdapi/saleAuthPost", Getkey ());
                if (_Re.success) {
                    _clientNotification.AddSweetNotification ("สำเร็จ",
                        "บันทึกข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                } else {
                    _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                    return RedirectToAction (nameof (Create));
                }
            } else {
                SaleAuth _SaleAuthUp = new SaleAuth () {
                    SaleId = _SaleAuth.SaleId,
                    RegionCode = _SaleAuth.RegionCode,
                    CompCode = _SaleAuth.CompCode,
                    Position = _SaleAuth.Position,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_SaleAuthUp, "http://192.168.10.46/sdapi/sdapi/saleAuthPut/" + _SaleAuth.CompCodeEdit + "/" + _SaleAuth.SaleIdEdit + "/" + _SaleAuth.RegionCodeEdit, Getkey ());
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
            return RedirectToAction (nameof (Create), new { _Id = _SaleAuth.SaleId });
        }

        [DisplayName ("แก้ไขนักเกษตร")]
        [HttpGet]
        public IActionResult EditMutiple (string Id, string RegId, string SaleId) {
            ViewBag.IsEditMode = "true";
            //*- หา _ReOndesale หารหัสที่จะแก้ไข  saleviewmodel จะเก็บได้ทั้ง models และ list หาทั้งของ saleid ว่ามีกี่อันที่จะเอาไปโชว์หน้าแก้ไข
            List<SaleAuth> saleview = new List<SaleAuth> ();
            SaleAuthViewmodel _Salestamodel = new SaleAuthViewmodel ();
            var _ReOneSale = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (saleview, "http://192.168.10.46/sdapi/sdapi/SaleAuthGet/" + Id + "/" + SaleId + "/" + RegId, Getkey ());
            var saleviewmodel = ServiceExtension.RestshapExtension.CallRestApiGET (saleview, "http://192.168.10.46/sdapi/sdapi/SaleAuthGet/" + SaleId, Getkey ());
            _Salestamodel.Salelist = saleviewmodel;
            foreach (var item in _ReOneSale) {
                _Salestamodel.CompCode = item.CompCode;
                _Salestamodel.SaleId = item.SaleId;
                _Salestamodel.RegionCode = item.RegionCode;
                _Salestamodel.Position = item.Position;
                _Salestamodel.SaleIdEdit = SaleId;
                _Salestamodel.CompCodeEdit = Id;
                _Salestamodel.RegionCodeEdit = RegId;

            }
            //*Dropdown sale ---------------------------

            List<SaleAuth> Drsale = new List<SaleAuth> ();
            var _Resale = ServiceExtension.RestshapExtension.CallRestApiGET (Drsale, "http://192.168.10.46/sdapi/sdapi/saleGet", Getkey ());
            var salefull = new List<SaleAuth> ();
            foreach (var sname in _Resale) {
                salefull.Add (new SaleAuth {
                    SaleId = sname.SaleId,
                        SaleFullname = sname.SaleId + " " + sname.SaleName
                });
            }
            //*Dropdown region ---------------------------
            List<SaleAuth> Drbranch = new List<SaleAuth> ();
            var _Reregion = ServiceExtension.RestshapExtension.CallRestApiGET (Drbranch, "http://192.168.10.46/sdapi/sdapi/regionGet", Getkey ());
            var branch = new List<SaleAuth> ();
            foreach (var users in _Reregion) {
                branch.Add (new SaleAuth {
                    RegionCode = users.RegionCode,
                        RegCodeAndName = users.RegionCode + " " + users.NameTH
                });

            }
            ViewBag.sale = salefull;
            ViewBag.branch = branch;

            return View ("Create", _Salestamodel);
        }

        [DisplayName ("ลบนักเกษตร")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1) {
            SaleAuth _SaleAuth = new SaleAuth ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_SaleAuth, "http://192.168.10.46/sdapi/sdapi/SaleAuthDel/" + Id + "/" + Del + "/" + Del1, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}