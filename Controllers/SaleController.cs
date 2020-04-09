using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SugarM.Models;
using SugarM.Repository;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    public class SaleController : BaseController<Companysale> {
        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public SaleController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public IActionResult Index () {
            List<Companysale> AuthorList = new List<Companysale> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet", Getkey ());

            return View (Call);
        }
        public IActionResult Create () {
            //*Dropdown sale ---------------------------
            List<Companysale> AuthorList = new List<Companysale> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet", Getkey ());
            ViewBag.sale = Call;
            List<Companysale> Companyget = new List<Companysale> ();
            var Call1 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget, "http://192.168.10.46/sdapi/sdapi/companyGet", Getkey ());
            List<Companysale> Companyget1 = new List<Companysale> ();
            var Call2 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/branchGet", Getkey ());
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
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet/" + Id, Getkey ());
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
        public async Task<IActionResult> Create (Companysale _Companysalemodel, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_Companysalemodel, "http://192.168.10.46/sdapi/sdapi/SalePost", Getkey ());
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
                Companysale _sale = new Companysale () {
                    SaleId = _Companysalemodel.SaleId,
                    SaleName = _Companysalemodel.SaleName,
                    CompCode = _Companysalemodel.CompCode,
                    BranchCode = _Companysalemodel.BranchCode,
                    UpdateBy = _UserProfile.EmployeeId,
                    Version = 0,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_sale, "http://192.168.10.46/sdapi/sdapi/SalePut/" + _Companysalemodel.SaleId, Getkey ());
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
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/saleGet/" + Id, Getkey ());
            foreach (var item in Call) {
                _CmpSale.SaleId = item.SaleId;
                _CmpSale.SaleName = item.SaleName;
                _CmpSale.CompCode = item.CompCode;
                _CmpSale.BranchCode = item.BranchCode;
            }
            //*Dropdown sale ---------------------------
            List<Companysale> _Autdorp = new List<Companysale> ();
            var _rec = ServiceExtension.RestshapExtension.CallRestApiGET (_Autdorp, "http://192.168.10.46/sdapi/sdapi/saleGet", Getkey ());
            ViewBag.sale = _rec;
            List<Companysale> Companyget = new List<Companysale> ();
            var _rec1 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget, "http://192.168.10.46/sdapi/sdapi/companyGet", Getkey ());
            List<Companysale> Companyget1 = new List<Companysale> ();
            var _rec2 = ServiceExtension.RestshapExtension.CallRestApiGET (Companyget1, "http://192.168.10.46/sdapi/sdapi/branchGet", Getkey ());
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
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_Companysale, "http://192.168.10.46/sdapi/sdapi/SaleDel/" + Id, Getkey ());
            return Json (new { success = Call.success });
        }
    }
}