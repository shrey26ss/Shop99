using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IRequestInfo
    {
        string GetRemoteIP();
        string GetLocalIP();
        string GetBrowser();
        string GetBrowserVersion();
        string GetUserAgent();
        string GetBrowserFullInfo();
        string GetDomain();
        string GetUserAgentMD5();
        string GetAbsoluteURI();
    }
}
