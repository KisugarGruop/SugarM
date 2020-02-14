using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Pages
{
    public static class MainMenu
    {
    
        public static class Dashboard
        {
            public const string PageName = "Dashboard Main";
            public const string RoleName = "Dashboard Main";
            public const string Path = "/Dashboard/Index";
            public const string ControllerName = "Dashboard";
            public const string ActionName = "Index";
        }

        public static class User
        {
            public const string PageName = "User";
            public const string RoleName = "User";
            public const string Path = "/UserRole/Index";
            public const string ControllerName = "UserRole";
            public const string ActionName = "Index";
        }
    }
}
