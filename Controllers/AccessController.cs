using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SugarM.Data;
using SugarM.Models;

namespace SugarM.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccessController(
            ApplicationDbContext dbContext,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        // GET: Access
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAccess()
        {
            var query = await (
                from user in _dbContext.Users join ur in _dbContext.UserRoles on user.Id equals ur.UserId into userRoles from userRole in userRoles.DefaultIfEmpty() join rle in _dbContext.Roles on userRole.RoleId equals rle.Id into roles from role in roles.DefaultIfEmpty() select new { user, userRole, role }
            ).ToListAsync();
            List<string> names = new List<string>();
            var userList = new List<UserRoleViewModel>();

            foreach (var grp in query.GroupBy(q => q.user.Id))
            {

                var first = grp.First();
                //เอา user id ไปค้นหา ว่ามีกี่รูน
                var user = await _userManager.FindByIdAsync(first.user.Id);
                var userRoleIds = await _userManager.GetRolesAsync(user);
                //แอด เข้า list string name เพื่อ เอาไปต่อกับ userRoleviewmodel
                foreach (var users in userRoleIds)
                {
                    if (users != null)
                    {
                        names.Add(users);
                    }
                }
                userList.Add(new UserRoleViewModel
                {
                    UserId = first.user.Id,
                    UserName = first.user.UserName,
                    Rname = names.Count == 0 ? "No Role" : string.Join(" | ", names),
                    Roles = first.role != null ? grp.Select(g => g.role).Select(r => r.Name) : new List<string>()
                });
                //เคลีย list ทิ้งก่อนไปวนใหม่
                names.Clear();
            }

            return new JsonResult(userList);
        }

        [HttpPost]
        public JsonResult Delect(string Id)
        {
            return Json(new { success = true, message = "ลบข้อมูลสำเร็จ" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = userRoles
            };

            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["Roles"] = roles;

            return View(userViewModel);
        }
        // POST: Access/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View(viewModel);
            }

            var user = await _dbContext.Users.FindAsync(viewModel.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (viewModel.Roles != null)
            {
                await _userManager.AddToRolesAsync(user, viewModel.Roles);
                ViewData["toastr"] = "add";
            }
            else
            {
                ViewData["toastr"] = "edit";
            }

            return RedirectToAction("Index");
        }
    }
}