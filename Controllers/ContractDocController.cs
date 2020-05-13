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
    public class ContractDocController : BaseController<ContractDoc> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public ContractDocController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }

        [DisplayName ("เพิ่มประเภทย่อย")]
        public IActionResult CreateDetaill (string CompCode, string CaneYear, string ContractCode) {
            ViewBag.IsEditMode = "false";
            ///** Query หาข้อมูลมา
            List<ContractType> AuthorListdetail = new List<ContractType> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/ContractTypeGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&ContractCode=" + ContractCode, Getkey ());
            //**  ดึงข้อมูลมาลง datatable
            List<ContractDoc> AuthorListdetailDoc = new List<ContractDoc> ();
            ContractDoc _ContractDocDetaill = new ContractDoc ();
            var CallDoc = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorListdetailDoc, "http://192.168.10.46/sdapi/sdapi/ContractDocDetailGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&ContractCode=" + ContractCode, Getkey ());
            foreach (var item in Call) {
                _ContractDocDetaill.CompCode = item.CompCode;
                _ContractDocDetaill.CaneYear = item.CaneYear;
                _ContractDocDetaill.ContyeName = item.Description;
                _ContractDocDetaill.Description = "";
                _ContractDocDetaill.ContractCode = item.ContractCode;
                _ContractDocDetaill.DocName = item.ContractCode;
                _ContractDocDetaill.Active = item.Active;
            }
            _ContractDocDetaill.ContractDoclist = CallDoc;

            //** ทำ Dropdowmlist ประเภทเอกสาร
            List<DocumentType> AuthorList = new List<DocumentType> ();
            var _ReDocmen = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocumentTypeGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear, Getkey ());
            var ContrctDoc = new List<ContractDoc> ();
            foreach (var sname in _ReDocmen) {
                ContrctDoc.Add (new ContractDoc {
                    DocCode = sname.DocCode,
                        NameMap = sname.DocCode + " | " + sname.DocName
                });
            }
            ViewBag.Mapname = ContrctDoc;
            return View ("CreateDetaill", _ContractDocDetaill);
        }

        [HttpPost]
        [DisplayName ("บันทึกเอกสารประกอบสัญญา")]
        public async Task<IActionResult> SaveCarTypeDetaill (ContractDoc _Cardetaill, string IsEditMode) {
            var UserCurrent = GetCurrentUser ();
            var UserCurrenCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            if (ModelState.IsValid) {
                if (IsEditMode.Equals ("false")) {
                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_Cardetaill, "http://192.168.10.46/sdapi/sdapi/ContractDocPost", Getkey ());
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
                    ContractDoc _ContractDocUP = new ContractDoc () {
                        CompCode = _Cardetaill.CompCode,
                        CaneYear = _Cardetaill.CaneYear,
                        DocCode = _Cardetaill.DocCode,
                        ContractCode = _Cardetaill.ContractCode,
                        Description = _Cardetaill.Description,
                        Active = _Cardetaill.Active,
                        DeleteFlag = _Cardetaill.DeleteFlag,
                        UpdateBy = _UserProfile.EmployeeId,
                        UpdateDate = ConvertDatetime (DateTime.UtcNow)
                    };

                    var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_ContractDocUP, "http://192.168.10.46/sdapi/sdapi/ContractDocPut?CompCode=" + _Cardetaill.CompCode + "&CaneYear=" + _Cardetaill.CaneYear + "&ContractCode=" + _Cardetaill.ContractCode + "&DocCode=" + _Cardetaill.DocCode, Getkey ());
                    if (_Re.success) {
                        _clientNotification.AddSweetNotification ("สำเร็จ",
                            "แก้ไขข้อมูลเรียบร้อยแล้ว",
                            NotificationHelper.NotificationType.success);
                        return RedirectToAction (nameof (CreateDetaill), new { CompCode = _Cardetaill.CompCode, CaneYear = _Cardetaill.CaneYear, ContractCode = _Cardetaill.ContractCode });
                    } else {
                        _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                            _Re.message,
                            NotificationHelper.NotificationType.error);
                    }
                }
                return RedirectToAction (nameof (CreateDetaill), new { CompCode = _Cardetaill.CompCode, CaneYear = _Cardetaill.CaneYear, ContractCode = _Cardetaill.ContractCode });
            }
            return View ("CreateDetaill", _Cardetaill);
        }

        [DisplayName ("แก้เอกสารประกอบสัญญา")]
        [HttpGet]
        public IActionResult EditDetaill (string CompCode, string CaneYear, string ContractCode, string DocCode) {
            ViewBag.IsEditMode = "true";
            string DocCodeOne = "";
            string DocCodeId = "";
            //** ทำ Dropdowmlist ประเภทเอกสาร
            List<DocumentType> AuthorList = new List<DocumentType> ();
            var _ReDocmen = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/DocumentTypeGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&DocCode=" + DocCode, Getkey ());
            foreach (var sname in _ReDocmen) {
                DocCodeId = sname.DocCode;
                DocCodeOne = sname.DocCode + "  |  " + sname.DocName;
            }
            ///** Query หาข้อมูลมา
            List<ContractType> AuthorListdetail = new List<ContractType> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorListdetail, "http://192.168.10.46/sdapi/sdapi/ContractTypeGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&ContractCode=" + ContractCode, Getkey ());
            //**  ดึงข้อมูลมาลง datatable
            List<ContractDoc> AuthorListdetailDoc = new List<ContractDoc> ();
            ContractDoc _ContractDocDetaill = new ContractDoc ();
            var CallDoc = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorListdetailDoc, "http://192.168.10.46/sdapi/sdapi/ContractDocDetailGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&ContractCode=" + ContractCode, Getkey ());
            //** ดึงแค่รายการเดียวจะเอา Description ของ ContactDoc
            ContractDoc _ContractDocDetaillOne = new ContractDoc ();
            var CallDocOne = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorListdetailDoc, "http://192.168.10.46/sdapi/sdapi/ContractDocGet?CompCode=" + CompCode + "&CaneYear=" + CaneYear + "&ContractCode=" + ContractCode + "&DocCode=" + DocCode, Getkey ());
            string ConDocDescription = "";
            string ConDocActive = "";

            foreach (var item in CallDocOne) {
                ConDocDescription = item.Description;
                ConDocActive = item.Active;

            }
            foreach (var item in Call) {
                _ContractDocDetaill.CompCode = item.CompCode;
                _ContractDocDetaill.CaneYear = item.CaneYear;
                _ContractDocDetaill.ContyeName = item.Description;
                _ContractDocDetaill.Description = ConDocDescription;
                _ContractDocDetaill.ContractCode = item.ContractCode;
                _ContractDocDetaill.DocName = item.ContractCode;
                _ContractDocDetaill.Active = ConDocActive;
                _ContractDocDetaill.NameMap = DocCodeOne;
                _ContractDocDetaill.DocCode = DocCodeId;
            }

            _ContractDocDetaill.ContractDoclist = CallDoc;
            return View ("CreateDetaill", _ContractDocDetaill);
        }

        [DisplayName ("ลบเอกสารประกอบสัญญา")]
        [HttpGet]
        public IActionResult Delete (string Id, string Del, string Del1, string Del2) {
            SaleAuth _SaleAuth = new SaleAuth ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST (_SaleAuth, "http://192.168.10.46/sdapi/sdapi/ContractDocDel?CompCode=" + Id + "&CaneYear=" + Del + "&ContractCode=" + Del1 + "&DocCode=" + Del2, Getkey ());
            return Json (new { success = Call.success });
        }
    }

}