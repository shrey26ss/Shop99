using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class DashboardTopBoxCount
    {
        public int TotalOrders { get; set; }
        public int ConfirmedOrder { get; set; }
        public int LowStocks { get; set; }
        public int TotalCustomer { get; set; }
    }
}
