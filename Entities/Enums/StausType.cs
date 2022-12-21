using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities.Enums
{
    public enum StausType
    {
        [Description("Order Placed")]
        OrderPlaced = 1,
        [Description("Order Canceled")]
        Cancel = 2,
        [Description("Order Confirmed")]
        Confirmed = 3,
        [Description("Order Delivered")]
        Delivered = 4,
        [Description("Product Shipped")]
        Shipped = 5
    }
}
