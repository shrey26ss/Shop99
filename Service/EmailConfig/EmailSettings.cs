using System;
using System.Collections.Generic;
using System.Text;

namespace Service.EmailConfig
{
    public class EmailSettings : EmailConfig
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
