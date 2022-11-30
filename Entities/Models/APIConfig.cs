using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class APIConfig
    {
        public string token { get; set; }
        public string baseUrl { get; set; }
        public string qrUrl { get; set; }
        public bool isWriteJsonToFile { get; set; }
    }
}
