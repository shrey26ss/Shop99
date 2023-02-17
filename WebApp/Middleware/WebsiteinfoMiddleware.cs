using AppUtility.APIRequest;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Middleware
{
    public class WebsiteinfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _memoryCache;
        public WebsiteinfoMiddleware(RequestDelegate next, AppSettings appSettings, IMemoryCache memoryCache) //OSInfo os,
        {
            _next = next;
            _appSettings = appSettings;
            _memoryCache=memoryCache;
        }
        public async Task Invoke(HttpContext context)
        {
            Webinfo oSInfo = new Webinfo(_appSettings, _memoryCache);
            var list = oSInfo.GetList().Result;
            if (list!=null)
            {
                var first = list.FirstOrDefault() ?? new WebsiteinfoModel();
                Websiteinfomation.Whitelogo = first.Whitelogo;
                Websiteinfomation.Coloredlogo = first.Coloredlogo;
                Websiteinfomation.Companyname = first.Companyname;
                Websiteinfomation.Companydomain = first.Companydomain;
                Websiteinfomation.CompanyemailID = first.CompanyemailID;
                Websiteinfomation.Companymobile = first.Companymobile;
                Websiteinfomation.Companyaddress = first.Companyaddress;
                Websiteinfomation.Footerdescription = first.Footerdescription;
            }
            await _next(context);
        }
    }
    public class Webinfo
    {
        private string _apiBaseURL;
        private readonly IMemoryCache _memoryCache;
        public Webinfo(AppSettings appSettings ,IMemoryCache memoryCache)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _memoryCache= memoryCache;
        }
        public async Task<List<WebsiteinfoModel>> GetList(int Id = 0)
        {
            List<WebsiteinfoModel> list = new List<WebsiteinfoModel>();
            bool isExist = _memoryCache.TryGetValue(ChacheKeys.WebsiteinfoModel, out list);
            if (isExist)
            {
                return list;
            }
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Websiteinfo/GetDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }));
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<WebsiteinfoModel>>>(apiResponse.Result);
                list = deserializeObject.Result;
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(7));
                _memoryCache.Set(ChacheKeys.WebsiteinfoModel, list, cacheEntryOptions);
            }

            return list;
        }
    }
}
