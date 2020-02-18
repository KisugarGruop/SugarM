using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Serialization.Json;
using SugarM.Models;

namespace SugarM.Controllers {
    [Authorize]

    public class UserRoleController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleController (UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        public IActionResult Index () {
            return View ();
        }

        public async Task<IActionResult> UserProfile () {
            ApplicationUser user = await _userManager.GetUserAsync (User);
            return View (user);
        }
        public IActionResult getall () {

            return View ();
        }
    }
}