
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SugarM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SugarM.Services
{
    public class ControllerDiscovery: IControllerDiscovery
    {
        private List<ControllerInfo> _mvcControllers;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public ControllerDiscovery(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public IEnumerable<ControllerInfo> GetControllers()
        {
            if (_mvcControllers != null)
                return _mvcControllers;

            _mvcControllers = new List<ControllerInfo>();
            var items = _actionDescriptorCollectionProvider
                .ActionDescriptors.Items
                .Where(descriptor => descriptor.GetType() == typeof(ControllerActionDescriptor))
                .Select(descriptor => (ControllerActionDescriptor)descriptor)
                .GroupBy(descriptor => descriptor.ControllerTypeInfo.FullName)
                .ToList();

            foreach (var actionDescriptors in items)
            {
                if (!actionDescriptors.Any())
                    continue;

                var actionDescriptor = actionDescriptors.First();
                var controllerTypeInfo = actionDescriptor.ControllerTypeInfo;
                var currentController = new ControllerInfo
                {
                    AreaName = controllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue,
                    DisplayName = controllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                    Name = actionDescriptor.ControllerName,
                };

                var actions = new List<ActionInfo>();
                foreach (var descriptor in actionDescriptors.GroupBy(a => a.ActionName).Select(g => g.First()))
                {
                    var methodInfo = descriptor.MethodInfo;
                    if (IsProtectedAction(controllerTypeInfo, methodInfo))
                        actions.Add(new ActionInfo
                        {
                            ControllerId = currentController.Id,
                            Name = descriptor.ActionName,
                            DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                        });
                }

                if (actions.Any())
                {
                    currentController.Actions = actions;
                    _mvcControllers.Add(currentController);
                }
            }

            return _mvcControllers;
        }

        private static bool IsProtectedAction(MemberInfo controllerTypeInfo, MemberInfo actionMethodInfo)
        {
            if (actionMethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(true) != null)
                return false;

            if (controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
                return true;

            if (actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
                return true;

            return false;
        }
    }
}
