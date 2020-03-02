using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientNotifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using SugarM.Data;
using SugarM.Extension;
using SugarM.Filters;
using SugarM.Models;
using SugarM.Repository;
using SugarM.ServiceExtension;
using SugarM.Services;

namespace SugarM {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.ConfigDatabase (Configuration);
            services.IdentityConfig (Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense ("MjA2Mjk0QDMxMzcyZTM0MmUzMGprT0x5Z3RqK2hEVGFCV1hnWXlWNVNYODJpU0FES1AvV0dNelRqeGkyYW89");
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            /*    app.UseHttpsRedirection (); */ //ถ้าจะใช้ HTTPS มาเอาออก launchsettings ให้ไปเปลี่ยน applicationurl 
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseStaticHttpContext ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=UserRole}/{action=UserProfile}/{id?}");
            });
        }
    }
}