using Microsoft.AspNetCore.Mvc;

namespace SugarM.Controllers {
    public class BreadController : Controller {
        public IActionResult Index () {
            return View ();
        }
    }
}