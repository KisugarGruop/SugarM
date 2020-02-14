using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SugarM.Models;

namespace SugarM.Controllers {
    public class CompanyController : Controller {
        public IActionResult Index () {
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCompany (Company value) {

            if (value.Statusform == "Add") {
                var jsoncon = JsonConvert.SerializeObject (value);
                var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/companypost");
                client.Timeout = -1;
                var request = new RestRequest (Method.POST);
                request.AddHeader ("Content-Type", "application/json");
                request.AddParameter ("application/json", jsoncon.ToString (), ParameterType.RequestBody);
                IRestResponse response = client.Execute (request);
                if (response.IsSuccessful) {
                    return Json (new { success = true, message = "บันทึกสำเร็จ" });
                } else {
                    return Json (new { error = false, message = "บันทึกไม่สำเร็จ" });
                }
            } else {
                var jsoncon = JsonConvert.SerializeObject (value);
                var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/companyput/" + value.CompCode);
                client.Timeout = -1;
                var request = new RestRequest (Method.POST);
                request.AddHeader ("CompCode", value.CompCode);
                request.AddHeader ("Content-Type", "application/json");
                request.AddParameter ("application/json", jsoncon.ToString (), ParameterType.RequestBody);
                IRestResponse response = client.Execute (request);
                if (response.IsSuccessful) {
                    return Json (new { success = true, message = "แก้ไขสำเร็จ" });
                } else {
                    return Json (new { error = false, message = "แก้ไขไม่สำเร็จ" });
                }
            }

        }
        public IActionResult TEST () {
            return View ();
        }
    }

}