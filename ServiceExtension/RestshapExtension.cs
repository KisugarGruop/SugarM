using System;
using System.Collections.Generic;
using System.Linq;
using AspNetTicketBridge;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SugarM.Models;

namespace SugarM.ServiceExtension
{
    public static class RestshapExtension
    {

        public static List<T> CallRestApiGET<T>(this List<T> t, string Url, string tokenkey)
        {
            var client = new RestClient(Url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "bearer " + tokenkey + "");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            var token = JToken.Parse(response.Content);
            var output = t.ToList();
            if (token is JArray)
            {
                t = JsonConvert.DeserializeObject<List<T>>(response.Content);
                var _listtotall = t.ToList();
                return _listtotall;
            }
            return output;
        }
        public static List<T> CallRestApiGETEDIT<T>(List<T> t, string Url, string tokenkey)
        {
            var client = new RestClient(Url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "bearer " + tokenkey + "");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            var token = JToken.Parse(response.Content);
            var output = t.ToList();
            if (token is JArray)
            {
                t = JsonConvert.DeserializeObject<List<T>>(response.Content);
                var _listtotall = t.ToList();
                return _listtotall;
            }
            return output;
        }

        public static Messageapi CallRestApiPOST<T>(T data, string Url, string tokenkey)
        {
            var jsoncon = JsonConvert.SerializeObject(data);

            var client = new RestClient(Url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "bearer " + tokenkey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", jsoncon.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject m = JObject.Parse(response.Content);
            Boolean f = (Boolean)m["success"];
            string msg = m["message"].ToString();
            var msgData = new Messageapi { success = f, message = msg };
            if (f)
            {
                return msgData;
            }
            else
            {
                return msgData;
            }
        }
    }
}