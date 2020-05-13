using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AspNetTicketBridge;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SugarM.Models;

namespace SugarM.Controllers
{
    public class BaseController<T> : Controller where T : new()
    {

        [DisplayName("ฟังชั่นPaginated")]
        public List<T> PaginatedResult(List<T> t, int page, int rowsPerPage)
        {
            @ViewBag.TotalRecords = t.Count;
            @ViewBag.CurrentPage = page;

            var skip = (page - 1) * rowsPerPage;

            var paginatedResult = t.Skip(skip).Take(rowsPerPage).ToList();
            return paginatedResult;
        }

        [DisplayName("ฟังชั่นค้นหาคีย์")]
        public string Getkey()
        {
            var getkey = Request.Cookies["Authorization"];
            string validationKey = "9519CB28E5FDBD10FD7994FB3F6591789E31000D1F1A5C34343DB1297F5EF426DF3F436EFC0DA1F4C6F0D16EB7BA47422D57E79427B36036F4E52AA37446780E";
            string decryptionKey = "017942881237C93FE8440B4D245CA268377AAD569B872FA3BA63DF57EB8CEEFA";

            var ticket = MachineKeyTicketUnprotector.UnprotectOAuthToken(getkey, decryptionKey, validationKey);
            var newTicket = AuthenticationTicketConverter.Convert(ticket, "UserInfo");
            var result = AuthenticateResult.Success(newTicket);

            return getkey;
        }

        [DisplayName("ฟังชั่นแปลงวันที่ให้เป็นสติง")]
        public string ConvertDatetime(DateTime? Day)
        {
            string ConDate = Day.Value.ToString("yyyy-MM-dd");
            return ConDate;
        }
        [DisplayName("ฟังชั่นแปลงปีเป็นสตริง")]
        public string ConvertYear()
        {
            DateTime FristYear = DateTime.Today;
            DateTime LastYear = DateTime.Today.AddYears(1);
            string BasedFrist = FristYear.ToString("yyyy", new CultureInfo("th-TH"));
            string BasedLast = LastYear.ToString("yyyy", new CultureInfo("th-TH"));
            return BasedFrist + "/" + BasedLast;
        }
    }
}