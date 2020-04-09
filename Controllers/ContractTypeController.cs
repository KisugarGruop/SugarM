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
    public class ContractTypeController : BaseController<ContractType> {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser () => _userManager.GetUserName (HttpContext.User);
        private string GetCurrenCompCode () => _userManager.GetUserId (HttpContext.User);
        public ContractTypeController (IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager) {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var year = ConvertYear ();
            List<ContractType> AuthorList = new List<ContractType> ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/ContractTypeGet/" + _UserProfile.CompCode, Getkey ());
            return View (Call);
        }
        public async Task<IActionResult> Create () {
            var UserCurrenCompCode = GetCurrenCompCode ();
            string Year = ConvertYear ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCurrenCompCode);
            var ContractModel = new ContractType () {
                CompCode = _UserProfile.CompCode,
                CaneYear = Year,
            };
            ViewBag.IsEditMode = "false";
            return View (ContractModel);
        }

        [DisplayName ("บันทึกประเภทสัญญา")]
        [HttpPost]
        public async Task<ActionResult> Create (ContractType _ContractType, string IsEditMode) {
            var UserCompCode = GetCurrenCompCode ();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile (UserCompCode);
            var UserCurrenCompCode = GetCurrenCompCode ();
            if (IsEditMode.Equals ("false")) {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_ContractType, "http://192.168.10.46/sdapi/sdapi/ContractTypePost", Getkey ());
                if (_Re.success) {
                    _clientNotification.AddSweetNotification ("สำเร็จ",
                        "บันทึกข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                    return RedirectToAction ("CreateDetaill", "ContractDoc", new { CompCode = _ContractType.CompCode, CaneYear = _ContractType.CaneYear, ContractCode = _ContractType.ContractCode });
                } else {
                    _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                    return RedirectToAction (nameof (Index));
                }
            } else {
                ContractType _ContractTypeDetaill = new ContractType () {
                    CompCode = _ContractType.CompCode,
                    CaneYear = _ContractType.CaneYear,
                    ContractCode = _ContractType.ContractCode,
                    Description = _ContractType.Description,
                    Active = _ContractType.Active,
                    DeleteFlag = _ContractType.DeleteFlag,
                    UpdateBy = _UserProfile.EmployeeId,
                    UpdateDate = ConvertDatetime (DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (_ContractTypeDetaill, "http://192.168.10.46/sdapi/sdapi/ContractTypePut?CompCode=" + _ContractType.CompCode + "&CaneYear=" + _ContractType.CaneYear + "&ContractCode=" + _ContractType.ContractCode, Getkey ());
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

        [DisplayName ("แก้ไขเอกสาร")]
        [HttpGet]
        public IActionResult Edit (string CompCode, string Caneyear, string ContractCode) {
            ViewBag.IsEditMode = "true";

            List<ContractType> AuthorList = new List<ContractType> ();
            ContractType _ContractTypemodel = new ContractType ();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT (AuthorList, "http://192.168.10.46/sdapi/sdapi/ContractTypeGet?CompCode=" + CompCode + "&CaneYear=" + Caneyear + "&ContractCode=" + ContractCode, Getkey ());
            foreach (var item in Call) {
                _ContractTypemodel.CompCode = item.CompCode;
                _ContractTypemodel.CaneYear = item.CaneYear;
                _ContractTypemodel.ContractCode = item.ContractCode;
                _ContractTypemodel.Description = item.Description;
                _ContractTypemodel.Active = item.Active;
            }
            return View ("Create", _ContractTypemodel);
        }
    }
}