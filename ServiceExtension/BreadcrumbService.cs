using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Services
{
    public class BreadcrumbService : IViewContextAware
    {
        IList<Breadcrumb> breadcrumbs;

        public void Contextualize(ViewContext viewContext)
        {
            breadcrumbs = new List<Breadcrumb>();

            string area = $"{viewContext.RouteData.Values["area"]}";
            string controller = $"{viewContext.RouteData.Values["controller"]}";
            string action = $"{viewContext.RouteData.Values["action"]}";
            object id = viewContext.RouteData.Values["id"];
            string title = $"{viewContext.ViewData["Title"]}";

            breadcrumbs.Add(new Breadcrumb(area, controller, action, title, id));

            if (!string.Equals(action, "index", StringComparison.OrdinalIgnoreCase))
            {
                breadcrumbs.Insert(0, new Breadcrumb(area, controller, "index", title));
            }
        }

        public IList<Breadcrumb> GetBreadcrumbs()
        {
            return breadcrumbs;
        }
    }
}
