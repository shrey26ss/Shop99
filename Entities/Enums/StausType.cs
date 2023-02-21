using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities.Enums
{
    public enum StatusType
    {
        [Description("All")]
        All = 0,
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
        [Description("Order Replace Initiated")]
        OrderReplaceInitiated = 8,
        [Description("Replacement Accepted")]
        ReplacementAccepted = 9,
        [Description("Return Recived")]
        ReturnReceived = 10,
        [Description("Cancel Requested")]
        CancelRequest = 11,
        [Description("Cancel Rejected")]
        CancelRequestRejected = 12,
        [Description("Return Confirmred")]
        ReturnConfirmred = 13,
        [Description("Return Canceled")]
        ReturnCanceled = 14,
        [Description("Order Replaced")]
        OrderReplaced = 15,
        [Description("Replace Rejected")]
        ReplaceRejected = 16,
        [Description("Out For Delivery")]
        OutForDelivery = 17,
        [Description("Rejected")]
        Rejected = 18,
        [Description("Is Approved")]
        IsApproved = 19,
        [Description("Order Initiated")]
        OrderInitiated = 20
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
