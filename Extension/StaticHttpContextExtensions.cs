using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SugarM.Extension {
    public static class StaticHttpContextExtensions {
        /*
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        */
        //Context //สืบทอด http เพื่อให้หน้า htmlใช้งานได้
        public static IApplicationBuilder UseStaticHttpContext (this IApplicationBuilder app) {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor> ();
            System.Web.HttpContext.Configure (httpContextAccessor);
            return app;
        }
    }
}