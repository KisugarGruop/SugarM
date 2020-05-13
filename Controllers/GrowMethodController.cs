using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SugarM.Models;
using SugarM.Repository;
using SugarM.TagHelpers;

namespace SugarM.Controllers
{
    public class GrowMethodController : BaseController<GrowMethod>
    {

        private IClientNotification _clientNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memorycahe;
        private readonly IUserprofileRepository _IUserprofileRepository;
        private string GetCurrentUser() => _userManager.GetUserName(HttpContext.User);
        private string GetCurrenCompCode() => _userManager.GetUserId(HttpContext.User);
        public GrowMethodController(IClientNotification clientNotification, IUserprofileRepository IUserprofileRepository, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
        {
            //_context = context;
            _clientNotification = clientNotification;
            _userManager = userManager;
            _memorycahe = memoryCache;
            _IUserprofileRepository = IUserprofileRepository;
        }
        public async Task<IActionResult> Index()
        {

            var UserCompCode = GetCurrenCompCode();
            var _CompCode = await _IUserprofileRepository.GetUserProfile(UserCompCode);

            // var stopwatch = new Stopwatch ();
            // stopwatch.Start ();
            // List<GrowMethod> AuthorList = new List<GrowMethod> ();
            // List<GrowMethod> GrowMethod;
            // if (!_memorycahe.TryGetValue ("GrowMethod", out GrowMethod)) {
            //     var Call1 = ServiceExtension.RestshapExtension.CallRestApiGET (AuthorList, "http://192.168.10.46/sdapi/sdapi/GrowMethodGet?CompCode=" + _CompCode.CompCode + "&CaneYear=" + ConvertYear (), Getkey ());
            //     _memorycahe.Set ("GrowMethod", Call1);
            // }
            // GrowMethod = _memorycahe.Get ("GrowMethod") as List<GrowMethod>;
            // stopwatch.Stop ();
            // ViewBag.Totaltime = stopwatch.Elapsed;
            List<GrowMethod> AuthorList = new List<GrowMethod>();

            var Call1 = ServiceExtension.RestshapExtension.CallRestApiGET(AuthorList, "http://192.168.10.46/sdapi/sdapi/GrowMethodGet?CompCode=" + _CompCode.CompCode + "&CaneYear=" + ConvertYear(), Getkey());


            return View(Call1);
        }

        public async Task<IActionResult> Create()
        {
            var UserCompCode = GetCurrenCompCode();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile(UserCompCode);
            var Year = ConvertYear();
            var GrowMethodemodel = new GrowMethod()
            {
                CompCode = _UserProfile.CompCode,
                CaneYear = Year

            };
            ViewBag.IsEditMode = "false";
            return View(GrowMethodemodel);
        }

        [DisplayName("เพิ่มวิธีการปลูก")]
        [HttpPost]
        public async Task<IActionResult> Create(GrowMethod _GrowMethod, string IsEditMode)
        {
            var UserCompCode = GetCurrenCompCode();
            var _UserProfile = await _IUserprofileRepository.GetUserProfile(UserCompCode);
            if (IsEditMode.Equals("false"))
            {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_GrowMethod, "http://192.168.10.46/sdapi/sdapi/GrowMethodPost", Getkey());
                if (_Re.success)
                {
                    _clientNotification.AddSweetNotification("สำเร็จ",
                        "บันทึกข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                }
                else
                {
                    _clientNotification.AddSweetNotification("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                }
            }
            else
            {
                GrowMethod _GrowMethodeEdit = new GrowMethod()
                {
                    CompCode = _GrowMethod.CompCode,
                    CaneYear = _GrowMethod.CaneYear,
                    GrowCode = _GrowMethod.GrowCode,
                    DeleteFlag = _GrowMethod.DeleteFlag,
                    Description = _GrowMethod.Description,
                    Active = _GrowMethod.Active,
                    UpdateBy = _UserProfile.EmployeeId,
                    UpdateDate = ConvertDatetime(DateTime.UtcNow)
                };

                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST(_GrowMethodeEdit, "http://192.168.10.46/sdapi/sdapi/GrowMethodPut?CompCode=" + _GrowMethod.CompCode + "&CaneYear=" + _GrowMethod.CaneYear + "&GrowCode=" + _GrowMethod.GrowCode, Getkey());
                if (_Re.success)
                {
                    _clientNotification.AddSweetNotification("สำเร็จ",
                        "แก้ไขข้อมูลเรียบร้อยแล้ว",
                        NotificationHelper.NotificationType.success);
                }
                else
                {
                    _clientNotification.AddSweetNotification("ผิดพลาด !!",
                        _Re.message,
                        NotificationHelper.NotificationType.error);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [DisplayName("แก้ไขผู้อนุมัติ")]
        [HttpGet]
        public IActionResult Edit(string Compcode, string CaneYear, string GrowCode)
        {
            ViewBag.IsEditMode = "true";

            List<GrowMethod> AuthorList = new List<GrowMethod>();
            GrowMethod _GrowMethod = new GrowMethod();
            var Call = ServiceExtension.RestshapExtension.CallRestApiGETEDIT(AuthorList, "http://192.168.10.46/sdapi/sdapi/GrowMethodGet?CompCode=" + Compcode + "&CaneYear=" + CaneYear + "&GrowCode=" + GrowCode, Getkey());
            foreach (var item in Call)
            {
                _GrowMethod.CompCode = item.CompCode;
                _GrowMethod.CaneYear = item.CaneYear;
                _GrowMethod.GrowCode = item.GrowCode;
                _GrowMethod.Description = item.Description;
                _GrowMethod.Active = item.Active;
            }
            return View("Create", _GrowMethod);
        }

        [DisplayName("ลบเกรดจัดชั้นหนี้")]
        [HttpGet]
        public IActionResult Delete(string Id, string Del, string Del1)
        {
            GrowMethod _GrowMethod = new GrowMethod();
            var Call = ServiceExtension.RestshapExtension.CallRestApiPOST(_GrowMethod, "http://192.168.10.46/sdapi/sdapi/OverrideLevelDel?CompCode=" + Id + "&CaneYear=" + Del + "&OverrideLevel=" + Del1, Getkey());
            return Json(new { success = Call.success });
        }

    }
}