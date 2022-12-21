using AppUtility.Helper;
using Data;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repos
{
    public class RequestInfoServcie : IRequestInfo
    {
        private IDapperRepository _dapper;
        private readonly IHttpContextAccessor _accessor;
      private readonly  IConfiguration _Configuration;
        public RequestInfoServcie(IDapperRepository dapper, IHttpContextAccessor accessor, IConfiguration Configuration)
        {
            _accessor = accessor;
            _dapper = dapper;
            _Configuration = Configuration;
        }
        public string GetBrowser()
        {
            string UserAgent = GetBrowserInfo(GetUserAgent());
            if ((UserAgent ?? "").Contains("/"))
            {
                if (UserAgent.Split("/").Length > 0)
                {
                    if (UserAgent.Split("/")[0] == "")
                    {
                        return UserAgent.Split("/")[1];
                    }
                    return UserAgent.Split("/")[0];
                }
            }
            return UserAgent.Trim();
        }

        public string GetBrowserVersion()
        {
            string UserAgent = GetBrowserInfo(GetUserAgent());
            if ((UserAgent ?? "").Contains("/"))
            {
                if (UserAgent.Split("/").Length == 2)
                {
                    return UserAgent.Split("/")[1];
                }
            }
            return UserAgent;
        }

        public string GetBrowserFullInfo() => string.IsNullOrWhiteSpace(GetUserAgent()) == false ? (GetBrowserInfo(GetUserAgent())) : "";


        public string GetRemoteIP() => _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        public string GetLocalIP() => _accessor.HttpContext.Connection.LocalIpAddress.ToString();

        public string GetUserAgent() => _accessor.HttpContext.Request.Headers.ContainsKey("User-Agent") ? _accessor.HttpContext.Request.Headers["User-Agent"].ToString().ToLower() : null;

        public string GetBrowserInfo(string useragent)
        {
            string name = "", version = "";
            try
            {
                //RestSharp/106.2.1.0
                if (useragent.Contains(" "))
                {
                    string[] arr = useragent.Split(' ');
                    string lastelement = arr[arr.Length - 1].ToLower();
                    if (lastelement.Contains("safari"))
                    {
                        string secondlastelement = arr[arr.Length - 2].ToLower();
                        if (secondlastelement.Contains("version"))
                        {
                            name = lastelement.Split('/')[0].ToUpper();
                            version = secondlastelement.Split('/')[1].ToString();
                        }
                        else
                        if (secondlastelement.Contains("chrome"))
                        {
                            name = secondlastelement.Split('/')[0].ToUpper();
                            version = secondlastelement.Split('/')[1].ToString();
                        }
                    }
                    else
                    if (lastelement.Contains("firefox") || lastelement.Contains("edge"))
                    {
                        name = lastelement.Split('/')[0].ToUpper();
                        version = lastelement.Split('/')[1].ToString();
                    }
                    else
                    if (lastelement.Contains("gecko"))
                    {
                        name = "Internet Explorer";
                        string[] arrtl = arr[arr.Length - 3].ToLower().Split(' ');
                        string thirdlastelemnt = arrtl[arrtl.Length - 1].ToLower();
                        version = thirdlastelemnt.Split(':')[1].ToString().TrimEnd(')');
                    }
                }
                else if (useragent.Contains("/"))
                {
                    name = "";//useragent.Split('/')[0];
                    version = useragent.Split('/')[1];
                }
                if (!string.IsNullOrEmpty(useragent) && name == "")
                {
                    version = useragent;
                }

            }
            catch (Exception)
            {
            }
            return name + "/" + version;
        }

        public string GetDomain()
        {
            string Domain = _accessor.HttpContext.Request.Host.ToString().Contains("www.") ? _accessor.HttpContext.Request.Host.ToString().Replace("www.", "") : _accessor.HttpContext.Request.Host.ToString();
            Domain = Domain.Contains(":") ? Domain.Split(':')[0] : Domain;
            try
            {
                if (Domain == "localhost")
                {
                    if (_Configuration["DefaultSetting:Website"] != null)
                    {
                        Domain = _Configuration["DefaultSetting:Website"];
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Domain;
        }




        public string GetUserAgentMD5()
        {
            string userAgent = GetUserAgent();
            if (!string.IsNullOrEmpty(userAgent))
            {
                userAgent = HashEncryption.O.MD5Hash(userAgent);
            }
            return userAgent ?? string.Empty;
        }

        public string GetAbsoluteURI()
        {
            var request = _accessor.HttpContext.Request;
            return request.Scheme + "://" + request.Host + request.Path;
        }
    }
}
