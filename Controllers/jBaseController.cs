using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SugarM.Controllers
{
    public class jBaseController : Controller
    {
        string getkey;
        string url = "http://192.168.10.46/sdapi/sdapi/";
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult getTK()
        {
            getkey = Request.Cookies["Authorization"];
            return Json(new { xtk = getkey, xurl = url });
        }

    }
}