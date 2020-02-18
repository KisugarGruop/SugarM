using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using SugarM.Models;
using SugarM.Utility;

namespace SugarM.Controllers {
    [SessionAuthorize]
    public class CompanyController : Controller {

        public IActionResult Index () {
            //* Dropdown BanchType---------------------------
            string cookieValueFromReq = Request.Cookies["Authorization"];
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchTypeGet");
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + cookieValueFromReq + "");
            IRestResponse response = client.Execute (request);
            var dataty = JsonConvert.DeserializeObject (response.Content);
            List<combanchty> da = JsonConvert.DeserializeObject<List<combanchty>> (response.Content);
            ViewBag.Banch = da;
            //*Dropdown Comapny ---------------------------

            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCompany (Company value) {
            if (value.Statusform == "Add") {
                var jsoncon = JsonConvert.SerializeObject (value);
                var clientadd = new RestClient ("http://192.168.10.46/sdapi/sdapi/companyget/" + value.CompCode);
                clientadd.Timeout = -1;
                var requestadd = new RestRequest (Method.GET);
                requestadd.AddHeader ("Content-Type", "application/json");
                IRestResponse responseadd = clientadd.Execute (requestadd);
                //---------- Check dupicate Company -------------
                if (responseadd.IsSuccessful) {
                    return Json (new { success = true, message = "Dupicate" });
                } else {
                    //-------------- AddSave Company------------------
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
                }

            } else {
                //-------- Edit and save Company
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Savebranch (Company value) {

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