using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AspNetTicketBridge;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SugarM.Models;

namespace SugarM.Controllers {
    public class BaseController<T> : Controller where T : new () {
        [DisplayName ("Paginated")]
        public List<T> PaginatedResult (List<T> t, int page, int rowsPerPage) {
            @ViewBag.TotalRecords = t.Count;
            @ViewBag.CurrentPage = page;

            var skip = (page - 1) * rowsPerPage;

            var paginatedResult = t.Skip (skip).Take (rowsPerPage).ToList ();
            return paginatedResult;
        }

        [DisplayName ("ค้นหาคีย์")]
        public string Getkey () {
            var getkey = Request.Cookies["Authorization"];
            string validationKey = "9519CB28E5FDBD10FD7994FB3F6591789E31000D1F1A5C34343DB1297F5EF426DF3F436EFC0DA1F4C6F0D16EB7BA47422D57E79427B36036F4E52AA37446780E";
            string decryptionKey = "017942881237C93FE8440B4D245CA268377AAD569B872FA3BA63DF57EB8CEEFA";

            var ticket = MachineKeyTicketUnprotector.UnprotectOAuthToken (getkey, decryptionKey, validationKey);
            var newTicket = AuthenticationTicketConverter.Convert (ticket, "UserInfo");
            var result = AuthenticateResult.Success (newTicket);

            return getkey;
        }

        [DisplayName ("ติดต่อAPIGET")]
        public List<T> CallRestApiGET (List<T> t, string Url) {
            var client = new RestClient (Url);
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey () + "");
            request.AddHeader ("Content-Type", "application/json");
            IRestResponse response = client.Execute (request);
            t = JsonConvert.DeserializeObject<List<T>> (response.Content);
            var _listtotall = t.ToList ();
            return _listtotall;
        }

        [DisplayName ("ติดต่อAPIEDIT")]
        public List<T> CallRestApiGETEDIT (List<T> t, string Url) {
            var client = new RestClient (Url);
            client.Timeout = -1;
            var request = new RestRequest (Method.GET);
            request.AddHeader ("Authorization", "bearer " + Getkey () + "");
            request.AddHeader ("Content-Type", "application/json");
            IRestResponse response = client.Execute (request);
            t = JsonConvert.DeserializeObject<List<T>> (response.Content);
            var _listtotall = t.ToList ();
            return _listtotall;
        }

        [DisplayName ("ติดต่อAPIPOST")]
        public Messageapi CallRestApiPOST (T data, string Url) {
            var jsoncon = JsonConvert.SerializeObject (data);

            var client = new RestClient (Url);
            client.Timeout = -1;
            var request = new RestRequest (Method.POST);
            request.AddHeader ("Authorization", "bearer " + Getkey () + "");
            request.AddHeader ("Content-Type", "application/json");
            request.AddParameter ("application/json", jsoncon.ToString (), ParameterType.RequestBody);
            IRestResponse response = client.Execute (request);
            JObject m = JObject.Parse (response.Content);
            Boolean f = (Boolean) m["success"];
            string msg = m["message"].ToString ();
            var msgData = new Messageapi { success = f, message = msg };
            if (f) {
                return msgData;
            } else {
                return msgData;
            }
        }

        [DisplayName ("แปลงวันที่ให้เป็นสติง")]
        public string ConvertDatetime (DateTime? Day) {
            string ConDate = Day.Value.ToString ("yyyy-MM-dd");
            return ConDate;
        }

    }
}