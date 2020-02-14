using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SugarM.Models;
using SugarM.Services;

namespace SugarM.Controllers {

    [Authorize]
    [Route ("[controller]/[action]")]
    [DisplayName ("Role Management")]

    public class RoleController : Controller {
        private readonly IControllerDiscovery _mvcControllerDiscovery;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public string Uid;
        public RoleController (IControllerDiscovery mvcControllerDiscovery, RoleManager<ApplicationRole> roleManager) {
            _mvcControllerDiscovery = mvcControllerDiscovery;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index () {
            var roles = await _roleManager.Roles.ToListAsync ();

            return View (roles);
        }

        [Authorize (Roles = "Admin")]
        [DisplayName ("สร้างเมนู")]
        // GET: Role/Create
        public ActionResult Create () {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();

            return View ();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create (RoleViewModel viewModel) {
            if (!ModelState.IsValid) {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();
                return View (viewModel);
            }

            var role = new ApplicationRole { Name = viewModel.Name, Discriminator = viewModel.Name };
            if (viewModel.SelectedControllers != null && viewModel.SelectedControllers.Any ()) {
                foreach (var controller in viewModel.SelectedControllers)
                    foreach (var action in controller.Actions)
                        action.ControllerId = controller.Id;

                var accessJson = JsonConvert.SerializeObject (viewModel.SelectedControllers);
                role.Access = accessJson;
            }

            var result = await _roleManager.CreateAsync (role);
            if (result.Succeeded)
                return RedirectToAction (nameof (Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError ("", error.Description);

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();

            return View (viewModel);
        }

        public IActionResult Getdata () {

            return Ok (_mvcControllerDiscovery.GetControllers ());
        }
        public IActionResult CheckBox () {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();

            return View ();
        }

        [Authorize (Roles = "Admin")]
        // GET: Role/Edit/5
        public async Task<ActionResult> Edit (string id) {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();
            Uid = id; //เอามาเก็บ ไอดี เพื่่อเอาไปใช้ต่อใน post edit
            var role = await _roleManager.FindByIdAsync (id);
            if (role == null)
                return NotFound ();

            var viewModel = new RoleViewModel {
                Name = role.Name,
                SelectedControllers = JsonConvert.DeserializeObject<IEnumerable<ControllerInfo>> (role.Access)
            };
            ViewData["Uid"] = Uid;
            return View (viewModel);
        }

        // POST: Role/Edit/5
        [HttpPost]

        public async Task<ActionResult> Edit (string id, RoleViewModel viewModel) {
            if (!ModelState.IsValid) {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();
                return View (viewModel);
            }

            var role = await _roleManager.FindByIdAsync (id);
            if (role == null) {
                ModelState.AddModelError ("", "Role not found");
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();
                return View ();
            }

            role.Name = viewModel.Name;
            if (viewModel.SelectedControllers != null && viewModel.SelectedControllers.Any ()) {
                foreach (var controller in viewModel.SelectedControllers)
                    foreach (var action in controller.Actions)
                        action.ControllerId = controller.Id;

                var accessJson = JsonConvert.SerializeObject (viewModel.SelectedControllers);
                role.Access = accessJson;
            }

            var result = await _roleManager.UpdateAsync (role);
            if (result.Succeeded)
                return RedirectToAction (nameof (Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError ("", error.Description);

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();

            return View (viewModel);
        }

        // Delete: role/5
        [Authorize (Roles = "Admin")]
        [HttpDelete ("role/{id}")]
        public async Task<ActionResult> Delete (string id) {
            var role = await _roleManager.FindByIdAsync (id);
            if (role == null) {
                ModelState.AddModelError ("Error", "Role not found");
                return BadRequest (ModelState);
            }

            var result = await _roleManager.DeleteAsync (role);
            if (result.Succeeded)
                return Ok (new { });

            foreach (var error in result.Errors)
                ModelState.AddModelError ("Error", error.Description);

            return BadRequest (ModelState);
        }
    }
}