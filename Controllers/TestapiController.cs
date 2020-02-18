using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;

namespace SugarM.Controllers {

    public class TestapiController : Controller {
        IHttpContextAccessor httpContextAccessor;
        public TestapiController (IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index () {
            BearerToken token;
            var client = new RestClient ("http://192.168.10.46/sdapi/token");
            client.Timeout = -1;
            var request = new RestRequest (Method.POST);
            request.AddHeader ("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter ("grant_type", "password");
            request.AddParameter ("username", "Admin");
            request.AddParameter ("password", "123456");
            IRestResponse response = client.Execute (request);
            string content = response.Content;
            token = new JsonDeserializer ().Deserialize<BearerToken> (response);
            CookieOptions option = new CookieOptions ();
            if (token.AccessToken != null) {
                option.Expires = DateTime.Now.AddMilliseconds (10);
                Response.Cookies.Append ("Authorization", token.AccessToken, option);
                getval ();
            }

            return View ();
        }

        public string getval () {
            //read cookie from IHttpContextAccessor  
            string cookieValueFromContext = httpContextAccessor.HttpContext.Request.Cookies["Authorization"];

            return cookieValueFromContext;
        }

    }
}