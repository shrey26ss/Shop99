using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class UserWalletledger
    {
        public int LoginId { get; set; } 
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string TrType { get; set; }
        public decimal Opening { get; set; }
        public decimal Amount { get; set; }
        public decimal Closing { get; set; }
        public int TID { get; set; }
        public string ServiceName { get; set; }
        public DateTime EntryOn { get; set; }
    }
    public class UserWalletledgerRequest
    {
        public StatusType Status { get; set; } = 0;
    }
    public class UserWalletLedgerRequest
    {
        public string Phonenumber { get; set; }
        public int UserID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

}
