using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SugarM.Models {
    public class ApplicationRole : IdentityRole {

        public string Access { get; set; }
        public string Discriminator { get; set; }
    }
}