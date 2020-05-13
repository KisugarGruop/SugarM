using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Serialization.Json;
using SugarM.Data;
using SugarM.Models;
using SugarM.TagHelpers;

namespace SugarM.Controllers {
    [Authorize]

    public class UserRoleController : Controller {
        private IClientNotification _clientNotification;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleController (IClientNotification clientNotification,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager) {
            _clientNotification = clientNotification;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [DisplayName ("หน้าหลัก")]
        public IActionResult Index () {
            List<UserProfile> Items = new List<UserProfile> ();
            Items = _dbContext.UserProfile.ToList ();
            int Count = Items.Count ();
            return View (Items);
        }

        [DisplayName ("ประวัติส่วนตัว")]
        public async Task<IActionResult> UserProfile () {
            ApplicationUser user = await _userManager.GetUserAsync (User);
            return View (user);
        }

        [DisplayName ("แก้ไขประวัติส่วนตัว")]
        public async Task<IActionResult> ChangeUserProfile () {
            ApplicationUser user = await _userManager.GetUserAsync (User);
            return View (user);
        }

        [HttpPost]
        [DisplayName ("บันทึกประวัติส่วนตัว")]
        public async Task<IActionResult> ChangeUserProfile (UserProfile payload) {

            UserProfile profile = payload;
            var user = await _userManager.FindByIdAsync (profile.ApplicationUserId);
            user.UserName = profile.Email;
            user.NormalizedEmail = profile.Email.ToUpper ();
            user.Email = profile.Email;
            user.NormalizedUserName = profile.Email;
            var userprodileedit = _dbContext.UserProfile.FirstOrDefault (a => a.ApplicationUserId == profile.ApplicationUserId);
            userprodileedit.FirstName = profile.FirstName;
            userprodileedit.LastName = profile.LastName;
            userprodileedit.Email = profile.Email;
            var result = await _dbContext.SaveChangesAsync ();
            ApplicationUser user1 = await _userManager.GetUserAsync (User);
            _clientNotification.AddSweetNotification ("สำเร็จ",
                "แก้ไขข้อมูลสำเร็จ",
                NotificationHelper.NotificationType.success);

            return View ("ChangeUserProfile", user1);
        }

        [DisplayName ("แก้ไขรหัสผ่าน")]
        public async Task<IActionResult> ChangePassword () {
            ApplicationUser user = await _userManager.GetUserAsync (User);
            return View (user);
        }

        [HttpPost]
        [DisplayName ("บันทึกการแก้ไขรหัสผ่าน")]
        public async Task<IActionResult> ChangePassword (UserProfile payload) {

            if (payload.Password.Equals (payload.ConfirmPassword)) {
                ApplicationUser useredit = await _userManager.FindByIdAsync (payload.ApplicationUserId);
                if (useredit == null) {
                    _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                        "ไม่พบUser ในระบบ!!",
                        NotificationHelper.NotificationType.error);
                }
                useredit.PasswordHash = _userManager.PasswordHasher.HashPassword (useredit, payload.Password);
                var result = await _userManager.UpdateAsync (useredit);
                _clientNotification.AddSweetNotification ("สำเร็จ",
                    "แก้ไขข้อมูลสำเร็จ",
                    NotificationHelper.NotificationType.success);
            } else {
                _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                    "กรุณาตรวจสอบพาสเวิดเก่าผิด!!",
                    NotificationHelper.NotificationType.error);
            }
            ApplicationUser usernew = await _userManager.GetUserAsync (User);
            return View ("ChangePassword", usernew);
        }

        [AllowAnonymous]
        [DisplayName ("บันทึกการแก้ไขรหัสผ่าน")]
        public async Task<IActionResult> SetDefultpassword (string Id) {

            ApplicationUser useredit = await _userManager.FindByIdAsync (Id);
            if (useredit == null) {
                _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                    "ไม่พบUser ในระบบ!!",
                    NotificationHelper.NotificationType.error);
            }
            useredit.PasswordHash = _userManager.PasswordHasher.HashPassword (useredit, "123456");
            var result = await _userManager.UpdateAsync (useredit);
            _clientNotification.AddSweetNotification ("สำเร็จ",
                "แก้ไขข้อมูลสำเร็จ",
                NotificationHelper.NotificationType.success);

            return RedirectToAction ("Index");
        }

        [DisplayName ("สร้างUser")]
        public IActionResult CreateUserAdmin () => View ();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserAdmin (User payload) {
            if (ModelState.IsValid) {
                ApplicationUser appUser = new ApplicationUser {
                    UserName = payload.UserName,
                    Email = payload.Email,
                };
                UserProfile userProfile = _dbContext.UserProfile.SingleOrDefault (x => x.EmployeeId.Equals (payload.EmployeeId));
                if (userProfile == null) {
                    /// สร้าง user
                    var Member = new ApplicationUser { UserName = payload.Email, Email = payload.Email };
                    IdentityResult result = await _userManager.CreateAsync (appUser, payload.Password);
                    ///กำหนดสิทธิ์ให้กับ user
                    ///
                    var Roleitem = new string[] { "member" };
                    var userRoleid = await _dbContext.Users.FindAsync (appUser.Id);
                    var userRoles = await _userManager.GetRolesAsync (userRoleid);
                    await _userManager.RemoveFromRolesAsync (userRoleid, userRoles);
                    var resultrole = await _userManager.AddToRolesAsync (userRoleid, Roleitem);
                    if (result.Succeeded && resultrole.Succeeded) {
                        UserProfile register = new UserProfile () {
                            ApplicationUserId = appUser.Id,
                            EmployeeId = payload.EmployeeId,
                            CompCode = payload.CompCode,
                            Password = appUser.PasswordHash,
                            FirstName = payload.FirstName,
                            LastName = payload.LastName,
                            Email = appUser.Email,
                            ConfirmPassword = appUser.PasswordHash,
                            OldPassword = appUser.PasswordHash,
                            ProfilePicture = "/upload/blank-person.png"
                        };
                        _dbContext.UserProfile.Add (register);
                        await _dbContext.SaveChangesAsync ();
                        //await _userManager.AddToRoleAsync(Member, "Member");
                        return RedirectToAction ("Index");
                    } else {
                        foreach (IdentityError error in result.Errors) {
                            ModelState.AddModelError ("", error.Description);
                        }
                    }
                } else {
                    _clientNotification.AddSweetNotification ("ผิดพลาด !!",
                        "รหัสพนักงานนี้มีอยู่ในระบบแล้ว",
                        NotificationHelper.NotificationType.error);
                }

            }
            return View (payload);
        }

    }
}