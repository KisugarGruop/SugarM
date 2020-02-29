using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SugarM.Components {
    [ViewComponent (Name = "ClientNotifications.Notify")]
    public class NotificationViewComponent : ViewComponent {
        public IViewComponentResult Invoke () {
            return View ("Notification");
        }
    }
}