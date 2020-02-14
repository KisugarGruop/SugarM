﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SugarM.Models;
using SugarM.Models.AccountViewModels;

namespace SugarM.Controllers {

    [Authorize]
    [Route ("[controller]/[action]")]
    public class AccountController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(Login login)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser appUser = await userManager.FindByNameAsync(login.UserName);
        //        if (appUser != null)
        //        {
        //            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);
        //            if (result.Succeeded)
        //            {
        //                return Redirect(login.ReturnUrl ?? "/");
        //            }
        //        }
        //        ModelState.AddModelError("", "เข้าสู่ระบบไม่สำเร็จ กรุณาตรวจสอบ UserName และ Password");
        //    }
        //    return View(login);
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login (string returnUrl = null) {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync (IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View ();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid) {
                //ApplicationUser appUser = await _userManager.FindByEmailAsync(model.Email);
                //if (appUser != null)
                //{

                //}
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync (model.Email, model.Password, false, false);
                if (result.Succeeded) {

                    return RedirectToLocal (returnUrl);
                } else {
                    ModelState.AddModelError (string.Empty, "Invalid login attempt.");
                    return View (model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View (model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout () {
            await _signInManager.SignOutAsync ();
            return RedirectToAction (nameof (HomeController.Index), "Home");
        }
        private IActionResult RedirectToLocal (string returnUrl) {
            if (Url.IsLocalUrl (returnUrl)) {
                return Redirect (returnUrl);
            } else {
                return RedirectToAction (nameof (HomeController.Index), "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult Getregis () => View ();
        [AllowAnonymous]
        public IActionResult Register () => View ();
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register (User user) {
            if (ModelState.IsValid) {
                ApplicationUser appUser = new ApplicationUser {
                    UserName = user.UserName,
                    Email = user.Email,
                };
                var Member = new ApplicationUser { UserName = user.Email, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync (appUser, user.Password);
                if (result.Succeeded) {
                    //await _userManager.AddToRoleAsync(Member, "Member");
                    return RedirectToAction ("Login");
                } else {
                    foreach (IdentityError error in result.Errors) {
                        ModelState.AddModelError ("", error.Description);
                    }
                }
            }
            return View (user);
        }

        [HttpGet]
        public IActionResult AccessDenied () {
            return View ();
        }
        public IActionResult Index () {
            return View ();
        }
    }
}