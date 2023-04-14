using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class TransactionStatusRequest
    {
        public int TID { get; set; }
        public string Status { get; set; }
    }
    public class PaymentDetails
    {
        public int TID { get; set; }
        public PaymentGatewayType PGID { get; set; }
        public decimal Amount { get; set; }
        public int UserID { get; set; }
        public string EntryOn { get; set; }
        public string Status { get; set; }
        public string UTR { get; set; }
        public int RefrenceID { get; set; }
        public decimal DebitedWalletAmount { get; set; }
    }
}
