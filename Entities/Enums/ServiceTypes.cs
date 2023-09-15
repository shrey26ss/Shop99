using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities.Enums
{
    public enum ServiceTypes
    {
        [Description("Self Transfer")]
        SelfTransfer = 1,
        [Description("Fund Transfer")]
        FundTransfer = 2,
        [Description("Fund Deduction")]
        FundDeduction = 3,
        [Description("Add Money")]
        AddMoney=4,
        [Description("Order")]
        Order = 5,
        [Description("Order Payment")]
        OrderPayment =6,
    }
}
