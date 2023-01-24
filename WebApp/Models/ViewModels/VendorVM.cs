using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class VendorVM : VendorProfile
    {
        public List<StateDDL> States { get; set; }
        public bool IsApproved { get; set; }
        public bool IsOnboard { get; set; }
    }
}
