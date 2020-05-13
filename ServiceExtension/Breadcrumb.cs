using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SugarM.Services
{
    public class Breadcrumb
    {
        public Breadcrumb(string area, string controller, string action, string title, object id) : this(area, controller, action, title)
        {
            Id = id;
        }

        public Breadcrumb(string area, string controller, string action, string title)
        {
            Area = area;
            Controller = controller;
            Action = action;

            if (string.IsNullOrWhiteSpace(title))
            {
                Title = Regex.Replace(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Equals(action, "Index", StringComparison.OrdinalIgnoreCase) ? controller : action), "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            }
            else
            {
                Title = title;
            }
        }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public object Id { get; set; }
        public string Title { get; set; }
    }
}
