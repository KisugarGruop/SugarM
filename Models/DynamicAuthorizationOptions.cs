using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Models
{
    public class DynamicAuthorizationOptions
    {
        /// <summary>
        /// Sets the default admin user. Authorization check will be suppressed.
        /// </summary>
        /// <value>The default admin user.</value>
        public string DefaultAdminUser { get; set; }
    }
}
