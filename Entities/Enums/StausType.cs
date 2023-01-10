using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities.Enums
{
    public enum StatusType
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
        Shipped = 5,
        [Description("Initiate Return")]
        ReturnInitiated = 6,
        [Description("Return Completed")]
        ReturnCompleted = 7,
        [Description("Order Replaced")]
        OrderReplaced = 8,
        [Description("Replacement Accepted")]
        ReplacementAccepted = 9,
    }
    public enum PurchaseType
    {
        [Description("Purchase Out")]
        Out = 1,
        [Description("Purchase In")]
        In = 0
    }
    public enum InventoryStatus
    {
        [Description("All Status")]
        All = 0,
        [Description("Low Stock")]
        LowStock = 1
    }
}
