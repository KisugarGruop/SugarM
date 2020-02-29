using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SugarM.Models;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class SaleController : BaseController<Companysale> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);

        public SaleController (IClientNotification clientNotification, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
        }
        public IActionResult Index () {
            List<Companysale> AuthorList = new List<Companysale> ();
            var Call = CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet");

            return View (Call);
        }
        public IActionResult Create () {
            //*Dropdown sale ---------------------------
            List<Companysale> AuthorList = new List<Companysale> ();
            var Call = CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet");
            ViewBag.sale = Call;
            List<Companysale> Companyget = new List<Companysale> ();
            var Call1 = CallRestApiGET (Companyget, "http://192.168.10.46/sdapi/sdapi/companyGet");
            List<Companysale> Companyget1 = new List<Companysale> ();
            var Call2 = CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/branchGet");
            var branch = new List<Companysale> ();
            foreach (var users in Call2) {
                branch.Add (new Companysale {
                    BranchCode = users.BranchCode,
                        DpComIdandname = users.BranchCode + " " + users.NameTH
                });

            }
            ViewBag.sale = Call;
            ViewBag.company = Call1;
            ViewBag.branch = branch;
            var _Salestamodel = new Companysale ();
            ViewBag.IsEditMode = "false";
            return View (_Salestamodel);
        }
        public IActionResult GetsaleID (string Id) {
            List<Companysale> AuthorList = new List<Companysale> ();
            var Call = CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet/" + Id);
            var saleList = new List<Companysale> ();
            foreach (var users in Call) {
                saleList.Add (new Companysale {
                    SaleName = users.SaleName
                });
            }
            ViewBag.IsEditMode = "false";
            return Json (saleList);
        }

        [DisplayName ("เพิ่มประเภทโควต้า")]
        [HttpPost]
        public IActionResult Create (Companysale _Companysalemodel, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            if (IsEditMode.Equals ("false")) {
                var _Re = CallRestApiPOST (_Companysalemodel, "http://192.168.10.46/sdapi/sdapi/SalePost");
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
                Companysale _Quota = new Companysale () {
                    SaleId = _Companysalemodel.SaleId,
                    SaleName = _Companysalemodel.SaleName,
                    CompCode = _Companysalemodel.CompCode,
                    BranchCode = _Companysalemodel.BranchCode,
                    UpdateBy = UserCurrent,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = CallRestApiPOST (_Quota, "http://192.168.10.46/sdapi/sdapi/SalePut/" + _Companysalemodel.SaleId);
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

        [DisplayName ("แก้ประเภทโควต้า")]
        [HttpGet]
        public IActionResult Edit (string Id) {
            ViewBag.IsEditMode = "true";

            List<Companysale> AuthorList = new List<Companysale> ();
            Companysale _CmpSale = new Companysale ();
            var Call = CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet/" + Id);
            foreach (var item in Call) {
                _CmpSale.SaleId = item.SaleId;
                _CmpSale.SaleName = item.SaleName;
                _CmpSale.CompCode = item.CompCode;
                _CmpSale.BranchCode = item.BranchCode;
            }
            //*Dropdown sale ---------------------------
            List<Companysale> _Autdorp = new List<Companysale> ();
            var _rec = CallRestApiGET (_Autdorp, "http://192.168.10.46/sdapi/sdapi/saleGet");
            ViewBag.sale = _rec;
            List<Companysale> Companyget = new List<Companysale> ();
            var _rec1 = CallRestApiGET (Companyget, "http://192.168.10.46/sdapi/sdapi/companyGet");
            List<Companysale> Companyget1 = new List<Companysale> ();
            var _rec2 = CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/branchGet");
            var branch = new List<Companysale> ();
            foreach (var users in _rec2) {
                branch.Add (new Companysale {
                    BranchCode = users.BranchCode,
                        DpComIdandname = users.BranchCode + " " + users.NameTH
                });

            }
            ViewBag.sale = Call;
            ViewBag.company = _rec1;
            ViewBag.branch = branch;

            return View ("Create", _CmpSale);
        }

        [DisplayName ("ลบประเภทโควต้า")]
        [HttpGet]
        public IActionResult Delete (string Id) {
            Companysale _Companysale = new Companysale ();
            var Call = CallRestApiPOST (_Companysale, "http://192.168.10.46/sdapi/sdapi/SaleDel/" + Id);
            return Json (new { success = Call.success });
        }
    }
}