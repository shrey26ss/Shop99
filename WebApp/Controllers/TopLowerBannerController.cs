using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode.Attributes;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class TopLowerBannerController : Controller
    {
        private string _apiBaseURL;
        public TopLowerBannerController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> TopLowerBannerList(int Id = 0)
        {
            return PartialView("PartialView/_TopLowerBannerList", await GetList(Id).ConfigureAwait(false));
        }

        public async Task<IActionResult> Edit(int Id = 0)
        {
            var res = new TopLowerBannerViewModel();
            if (Id > 0)
            {
                var resList = await GetList(Id);
                res = new TopLowerBannerViewModel
                {
                    Id = Id,
                    BackLinkText = resList.FirstOrDefault()?.BackLinkText,
                    BackLinkURL = resList.FirstOrDefault()?.BackLinkURL,
                    BannerPath = resList.FirstOrDefault()?.BannerPath,
                    Subtitle = resList.FirstOrDefault()?.Subtitle,
                    Title = resList.FirstOrDefault()?.Title
                };
            }
            return PartialView("PartialView/_AddLowerTopBanner", res);
        }
        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> Edit(TopLowerBannerViewModel model)
        {
            Response response = new Response();
            try
            {
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}.jpg";
                var _ = Utility.O.UploadFile(new FileUploadModel
                {
                    file = model.File,
                    FileName = fileName,
                    FilePath = FileDirectories.TopLowerBanner,
                });
                if (_.StatusCode == ResponseStatus.Success)
                {
                    string _token = User.GetLoggedInUserToken();
                    string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    model.BannerPath = $"{absoluteURL}/{FileDirectories.TopLowerBannerSuffix}/{fileName}";
                    var jsonData = JsonConvert.SerializeObject(model);
                    var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/TopLowerBanner/AddUpdate", jsonData, _token);
                    if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                        response = deserializeObject;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }

        private async Task<List<TopLowerBanner>> GetList(int Id = 0)
        {
            List<TopLowerBanner> list = new List<TopLowerBanner>();
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/TopLowerBanner/GetDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<TopLowerBanner>>>(apiResponse.Result);
                list = deserializeObject.Result;
            }
            return list;
        }
    }
}
