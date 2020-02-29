using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SugarM.Data;
using SugarM.Models;

namespace SugarM.Services {
    public class Functional : IFunctional {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Functional (UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<string> UploadFile (List<IFormFile> files, IWebHostEnvironment env, string uploadFolder) {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = System.IO.Path.Combine (webRoot, uploadFolder);
            var extension = "";
            var filePath = "";
            var fileName = "";

            foreach (var formFile in files) {
                if (formFile.Length > 0) {
                    extension = System.IO.Path.GetExtension (formFile.FileName);
                    fileName = Guid.NewGuid ().ToString () + extension;
                    filePath = System.IO.Path.Combine (uploads, fileName);

                    using (var stream = new FileStream (filePath, FileMode.Create)) {
                        await formFile.CopyToAsync (stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }

    }
}