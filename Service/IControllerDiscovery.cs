
using SugarM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarM.Services
{
    public interface IControllerDiscovery
    {
        IEnumerable<ControllerInfo> GetControllers();
    }
}
