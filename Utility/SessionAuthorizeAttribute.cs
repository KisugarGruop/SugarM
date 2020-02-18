using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SugarM.Extension;

namespace SugarM.Utility {
    public class SessionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter {
        public SessionAuthorizeAttribute () {

        }

        public async Task OnAuthorizationAsync (AuthorizationFilterContext context) {

            if (context.Filters.Any (item => item is IAllowAnonymousFilter)) {
                return;
            }

            string pl = context.HttpContext.Request.Cookies["Authorization"];
            if (pl == null) {
                context.Result = new RedirectToRouteResult (new RouteValueDictionary (new { controller = "Account", action = "Login" }));
            }

        }

    }
}