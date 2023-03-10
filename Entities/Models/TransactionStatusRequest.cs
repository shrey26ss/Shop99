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
}
