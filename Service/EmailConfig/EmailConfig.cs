using System;
using System.Collections.Generic;
using System.Text;

namespace Service.EmailConfig
{
    public class EmailConfig
    {
        public int Id { get; set; }
        public string EmailFrom { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string UserId { get; set; }
    }
}
