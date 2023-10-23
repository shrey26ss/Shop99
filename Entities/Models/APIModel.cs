using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class APIModel
    {
        public int Id { get; set; }
        public string TID { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string EntryOn { get; set; }
        public string Method { get; set; }
        public string InitiationTime { get; set; }
        public bool IsIncomingOutgoing { get; set; }
    }
}
