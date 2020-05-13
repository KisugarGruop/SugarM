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

    public class RoleController : Controller

    {
        private readonly IControllerDiscovery _mvcControllerDiscovery;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public string Uid;
        public RoleController (IControllerDiscovery mvcControllerDiscovery, RoleManager<ApplicationRole> roleManager) {
            _mvcControllerDiscovery = mvcControllerDiscovery;
            _roleManager = roleManager;
        }

        [DisplayName ("หน้าหลัก")]
        public async Task<IActionResult> Index () {
            var roles = await _roleManager.Roles.ToListAsync ();

            return View (roles);
        }

        [Authorize (Roles = "Admin")]
        [DisplayName ("สร้างสิทธิ์")]
        // GET: Role/Create
        public ActionResult Create () {

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();
            List<G_JSTree> G_JSTreeArray = new List<G_JSTree> ();
            int i = 1;
            foreach (var controller in _mvcControllerDiscovery.GetControllers ()) {
                G_JSTree _G_JSTree = new G_JSTree ();
                _G_JSTree.text = controller.Name;
                _G_JSTree.parent = "#";
                var ListChild = new List<G_Childern> ();
                foreach (var action in controller.Actions) {
                    i = i + 1;
                    ListChild.Add (new G_Childern {
                        id = Guid.NewGuid ().ToString (),
                            text = action.DisplayName,
                            parent = controller.Name,
                            state = new G_JsTreeAttribute { selected = false },
                            Icon = "fa fa-warning kt-font-danger"
                    });
                }
                i = i + 1;
                if (ListChild.Any ()) {
                    _G_JSTree.children = ListChild;
                    G_JSTreeArray.Add (_G_JSTree);
                }

            }
            ViewBag.Json = JsonConvert.SerializeObject (G_JSTreeArray);
            return View ();
        }
        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create (string seletedItems, string Name) {

            List<G_JSTreePreeview> item = JsonConvert.DeserializeObject<List<G_JSTreePreeview>> (seletedItems);
            List<G_JSTree> G_JSTreeArrayOutput = new List<G_JSTree> ();
            var ListChild = new List<G_Childern> ();
            item.ForEach (a => {
                if (!a.parent.Equals ("#")) {
                    ListChild.Add (new G_Childern {
                        text = a.text,
                            state = new G_JsTreeAttribute { selected = a.state.selected },
                            id = a.id,
                            parent = a.parent,
                            Icon = "fa fa-warning kt-font-danger"
                    });
                }
            });
            item.ForEach (x => {
                if (x.parent.Equals ("#")) {
                    var list = ListChild.Where (a => a.parent == x.id);
                    G_JSTreeArrayOutput.Add (new G_JSTree () {
                        text = x.text,
                            parent = x.parent,
                            children = list.ToList ()
                    });
                }
            });

            if (!ModelState.IsValid) {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();
                return View ("index");
            }

            var role = new ApplicationRole { Name = Name, Discriminator = Name };
            if (G_JSTreeArrayOutput != null && G_JSTreeArrayOutput.Any ()) {
                var accessJson = JsonConvert.SerializeObject (G_JSTreeArrayOutput);
                role.Access = accessJson;
            }

            var result = await _roleManager.CreateAsync (role);
            if (result.Succeeded)
                return RedirectToAction (nameof (Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError ("", error.Description);

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers ();

            return View ("index");
        }

        [Authorize (Roles = "Admin")]
        [DisplayName ("แก้ไขข้อมูล")]
        // GET: Role/Edit/5
        public async Task<ActionResult> Edit (string id) {

            Uid = id; //เอามาเก็บ ไอดี เพื่่อเอาไปใช้ต่อใน post edit
            var role = await _roleManager.FindByIdAsync (id);
            if (role == null)
                return NotFound ();

            var viewModel = new RoleViewModel {
                Name = role.Name

            };
            ViewBag.Json = role.Access;
            ViewData["Uid"] = Uid;
            return View (viewModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit (string seletedItems, string Name, string id) {
            if (!ModelState.IsValid) {

                return View ();
            }

            var role = await _roleManager.FindByIdAsync (id);
            if (role == null) {
                ModelState.AddModelError ("", "Role not found");
                return View ();
            }

            //************* Updatepost
            List<G_JSTreePreeview> item = JsonConvert.DeserializeObject<List<G_JSTreePreeview>> (seletedItems);
            List<G_JSTree> G_JSTreeArrayOutput = new List<G_JSTree> ();
            var ListChild = new List<G_Childern> ();
            item.ForEach (a => {
                if (!a.parent.Equals ("#")) {
                    ListChild.Add (new G_Childern {
                        text = a.text,
                            state = new G_JsTreeAttribute { selected = a.state.selected },
                            id = a.id,
                            parent = a.parent,
                            Icon = "fa fa-warning kt-font-danger"
                    });
                }
            });
            item.ForEach (x => {
                if (x.parent.Equals ("#")) {
                    var list = ListChild.Where (a => a.parent == x.id);
                    G_JSTreeArrayOutput.Add (new G_JSTree () {
                        text = x.text,
                            parent = x.parent,
                            children = list.ToList ()
                    });
                }
            });

            role.Name = Name;

            if (G_JSTreeArrayOutput != null && G_JSTreeArrayOutput.Any ()) {
                var accessJson = JsonConvert.SerializeObject (G_JSTreeArrayOutput);
                role.Access = accessJson;
            }

            var result = await _roleManager.UpdateAsync (role);
            if (result.Succeeded)
                return RedirectToAction (nameof (Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError ("", error.Description);

            return View ("Index");
        }

        // Delete: role/5
        [Authorize (Roles = "Admin")]
        [HttpDelete]
        [DisplayName ("ลบข้อมูล")]
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
    public class G_JSTree {
        public G_JsTreeAttribute state;
        public string text {
            get;
            set;
        }
        public string DisplayName {
            get;
            set;

        }

        public string parent {
            get;
            set;
        }
        public IEnumerable<G_Childern> children { get; set; }
    }

    public class G_JSTreePreeview {
        public G_JsTreeAttribute state;
        public string id {
            get;
            set;
        }
        public string text {
            get;
            set;
        }
        public string DisplayName {
            get;
            set;

        }

        public string parent {
            get;
            set;
        }
        public IEnumerable<G_Childern> children { get; set; }
    }
    public class G_Childern {
        public G_JsTreeAttribute state;
        public string id {
            get;
            set;
        }
        public string text {
            get;
            set;
        }
        public string Icon {
            get;
            set;
        }
        public string parent {
            get;
            set;
        }

    }

    public class G_JsTreeAttribute {
        public bool selected;
    }
}