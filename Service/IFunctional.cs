using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Services
{
    public interface IFunctional
    {
        Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env, string uploadFolder);

    }
}
