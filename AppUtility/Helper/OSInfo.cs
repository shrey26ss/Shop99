using AppUtility.APIRequest;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Linq;

namespace AppUtility.AppCode
{
    public class OSInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string FullInfo { get; set; }
        public OSInfo(IHttpContextAccessor accessor)
        {
            var ua = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
            ua = ua ?? string.Empty;
            ua = ua.Replace(" ", "");
            if (ua.Contains("Android", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Android";
                SetVersion(ua, "Android");
                this.FullInfo = this.Name; 
            }

            if (ua.Contains("iPhone", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "iPhone";
                SetVersion(ua, "OS");
                this.FullInfo = this.Name; 
            }

            if (ua.Contains("iPad", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "iPad";
                SetVersion(ua, "OS");
                this.FullInfo = this.Name; 
            }

            if (ua.Contains("MacOS", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "MacOS";
                this.FullInfo = this.Name; 
            }

            if (ua.Contains("WindowsNT10", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "10";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("WindowsNT6.3", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "8.1";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("WindowsNT6.2", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "8";
                this.FullInfo = this.Name + " " + this.Version; 
            }


            if (ua.Contains("WindowsNT6.1", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "7";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("WindowsNT6.0", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "Vista";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("WindowsNT5.1") || ua.Contains("WindowsNT5.2", StringComparison.OrdinalIgnoreCase))

            {
                this.Name = "Windows";
                this.Version = "XP";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("WindowsNT5", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "2000";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("WindowsNT4", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "NT4";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("Win9x4.90", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "Me";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("Windows98", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "98";
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Contains("Windows95", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows";
                this.Version = "95";
                this.FullInfo = this.Name + " " + this.Version; 
            }


            if (ua.Replace(" ", "").Contains("WindowsPhone", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Windows Phone";
                SetVersion(ua, "Windows Phone");
                this.FullInfo = this.Name + " " + this.Version; 
            }

            if (ua.Replace(" ", "").Contains("Linux") && ua.Contains("KFAPWI", StringComparison.OrdinalIgnoreCase))
            {
                this.Name = "Kindle Fire";
                this.FullInfo = this.Name; 
            }
            if (ua.Contains("PostmanRuntime"))
            {
                var d = ua.Split('/');
                this.Name = d[0];
                if (d.Count() > 1)
                {
                    this.Version = d[1];
                }
                this.FullInfo = this.Name + this.Version; 
            }

            if (ua.Contains("RIMTablet") || (ua.Contains("BB") && ua.Contains("Mobile", StringComparison.OrdinalIgnoreCase)))
            {
                this.Name = "Black Berry";
                this.FullInfo = this.Name; 
            }
            UserAgent.FullInfo = GetBrowserInfo(ua);
            UserAgent.Name = ua;
            UserAgent.Version = this.Version;
            //fallback to basic platform:
            //this.Name = request.Browser.Platform + (ua.Replace(" ","").Contains("Mobile") ? " Mobile " : "");
        }

        private void SetVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }
            this.Version = version;
        }

        private string GetBrowserInfo(string useragent)
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
                    if (lastelement.Contains("firefox") || lastelement.Contains("edge") || lastelement.Contains("edg"))
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
    }
}
