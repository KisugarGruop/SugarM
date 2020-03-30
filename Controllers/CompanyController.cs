using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SugarM.Models;
using SugarM.Utility;

namespace SugarM.Controllers {
    [SessionAuthorize]
    [Authorize (Roles = "Admin")]
    public class CompanyController : BaseController<Company> {

        public IActionResult Index () {
            //* Dropdown BanchType---------------------------
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchTypeGet");
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<combanchty> da = JsonConvert.DeserializeObject<List<combanchty>> (response.Content);
            ViewBag.Banch = da;
            //*Dropdown Comapny ---------------------------
            var clientsale = new RestClient ("http://192.168.10.46/sdapi/sdapi/saleGet");
            clientsale.Timeout = -1;
            var requestsale = new RestRequest (Method.GET);
            requestsale.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse responsesale = clientsale.Execute (requestsale);
            List<Companysale> sale = JsonConvert.DeserializeObject<List<Companysale>> (responsesale.Content);
            var saleList = new List<Companysale> ();
            foreach (var users in sale) {
                saleList.Add (new Companysale {
                    SaleId = users.SaleId,
                        SaleName = users.SaleId + " " + users.SaleName
                });

            }
            ViewBag.sale = saleList;
            return View ();
        }

        [Route ("Company/Getbanchregion/{Comid}/{Branid}")]
        [HttpGet]
        public IActionResult Getbanchregion (string Comid, string Branid) {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/villageGet/" + Comid + "/" + Branid);
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            var token = JToken.Parse (response.Content);

            if (token is JArray) {
                List<Village> da = JsonConvert.DeserializeObject<List<Village>> (response.Content);
                return Json (da);
            } else if (token is JObject) {
                return Json ("");
            }
            return Json ("");
        }

        [HttpGet]
        public IActionResult Getcompanyregion () {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/regionGet");
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<Company> data = JsonConvert.DeserializeObject<List<Company>> (response.Content);
            return Json (data);
        }

        [HttpGet]
        public IActionResult Getcompany () {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/companyGet");
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<Company> da = JsonConvert.DeserializeObject<List<Company>> (response.Content);
            return Json (da);
        }

        [Route ("Company/Getcompany/{Id}")]
        [HttpGet]
        public IActionResult Getcompany (string Id) {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/companyGet/" + Id);
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<Company> da = JsonConvert.DeserializeObject<List<Company>> (response.Content);
            return Json (da);
        }

        [HttpGet]
        public IActionResult GetcompanyRegionedit (string Id, string re) {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/regionGet/" + Id + "/" + re);
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<Company> da = JsonConvert.DeserializeObject<List<Company>> (response.Content);
            return Json (da);
        }

        [HttpGet]
        public IActionResult Getcompanybanch () {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchGet");
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<Company> da = JsonConvert.DeserializeObject<List<Company>> (response.Content);
            return Json (da);
        }

        [Route ("Company/Getcompanybanch/{banchid}/{comid}")]
        [HttpGet]
        public IActionResult Getcompanybanch (string banchid, string comid) {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchGet/" + banchid + "/" + comid);
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            List<Company> da = JsonConvert.DeserializeObject<List<Company>> (response.Content);
            return Json (da);
        }

        [Route ("Company/Delectcompany/{comid}")]
        [HttpPost]
        public IActionResult Delectcompany (string comid) {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/companydel/" + comid);
            client.Timeout = -1;
            var request = new RestRequest (Method.POST);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            JObject m = JObject.Parse (response.Content);
            Boolean f = (Boolean) m["success"];
            string msg = m["message"].ToString ();
            if (f) {
                return Json (new { success = true, message = msg });
            } else {
                return Json (new { success = false, message = msg });
            }
        }

        [Route ("Company/Delectbanch/{banchid}/{comid}")]
        [HttpPost]
        public IActionResult Delectbanch (string banchid, string comid) {
            var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchDel/" + comid + "/" + banchid);
            client.Timeout = -1;
            var request = new RestRequest (Method.POST);
            request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
            IRestResponse response = client.Execute (request);
            JObject m = JObject.Parse (response.Content);
            Boolean f = (Boolean) m["success"];
            string msg = m["message"].ToString ();
            if (f) {
                return Json (new { success = true, message = msg });
            } else {
                return Json (new { success = false, message = msg });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCompany (Company value) {
            if (value.Statusform == "Add") {
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (value, "http://192.168.10.46/sdapi/sdapi/companypost", Getkey ());
                return Json (new { success = _Re.success, message = _Re.message });

            } else {
                value.Version = 0;
                //-------- Edit and save Company
                var _Re = ServiceExtension.RestshapExtension.CallRestApiPOST (value, "http://192.168.10.46/sdapi/sdapi/companyput/" + value.CompCode, Getkey ());
                return Json (new { success = _Re.success, message = _Re.message });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCompanybranch (Company value) {
            if (value.Statusform == "Add") {
                //-------------- AddSave Company------------------
                var jsoncon = JsonConvert.SerializeObject (value);
                var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchPost");
                client.Timeout = -1;
                var request = new RestRequest (Method.POST);
                request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
                request.AddHeader ("Content-Type", "application/json");
                request.AddParameter ("application/json", jsoncon.ToString (), ParameterType.RequestBody);
                IRestResponse response = client.Execute (request);
                JObject m = JObject.Parse (response.Content);
                Boolean f = (Boolean) m["success"];

                string msg = m["message"].ToString ();
                if (f) {
                    return Json (new { success = true, message = msg });
                } else {
                    return Json (new { success = false, message = msg });
                }
            } else {
                //-------- Edit and save Company
                value.Version = 0;
                var jsoncon = JsonConvert.SerializeObject (value);
                var client = new RestClient ("http://192.168.10.46/sdapi/sdapi/branchPut/" + value.CompCode + "/" + value.BranchCode);
                client.Timeout = -1;
                var request = new RestRequest (Method.POST);
                request.AddHeader ("Authorization", "bearer " + Getkey1 () + "");
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
        public string Getkey1 () {
            var getkey = Request.Cookies["Authorization"];
            return getkey;
        }

    }

}