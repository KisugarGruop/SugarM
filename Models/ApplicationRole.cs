using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Models
{
    public class ApplicationRole: IdentityRole
    {
        public string Access { get; set; }
        public string Discriminator { get; set; }
    }
}
