using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Entity
{
    public class NotificationPermissions
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public List<string> Allowed { get; set; }
    }
}
