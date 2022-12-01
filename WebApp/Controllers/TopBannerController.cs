using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode.Attributes;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class TopBannerController : Controller
    {
        private string _apiBaseURL;
        public TopBannerController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> TopBannerList(int Id = 0)
        {
            return PartialView("PartialView/_TopBannerList", await GetList(Id).ConfigureAwait(false));
        }
        public async Task<IActionResult> Edit(int Id = 0)
        {
            var res = new TopBannerViewModel();
            if(Id > 0)
            {
                var resList = await GetList(Id);
                res = new TopBannerViewModel
                {
                    Id= Id,
                    BackLinkText = resList.FirstOrDefault()?.BackLinkText,
                    BackLinkURL = resList.FirstOrDefault()?.BackLinkURL,
                    BannerPath= resList.FirstOrDefault()?.BannerPath,
                    Subtitle= resList.FirstOrDefault()?.Subtitle,
                    Title= resList.FirstOrDefault()?.Title
                };
            }
            return PartialView("PartialView/_AddTopBanner", res);
        }
        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> Edit(TopBannerViewModel model)
        {
            Response response = new Response();
            try
            {
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}.jpg";
                var _ = Utility.O.UploadFile(new FileUploadModel
                {
                    file = model.File,
                    FileName = fileName,
                    FilePath = FileDirectories.TopBanner,
                });
                if (_.StatusCode == ResponseStatus.Success)
                {
                    string _token = User.GetLoggedInUserToken();
                    string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    model.BannerPath = $"{absoluteURL}/{FileDirectories.TopBannerSuffix}/{fileName}";
                    var jsonData = JsonConvert.SerializeObject(model);
                    var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/TopBanner/AddUpdate", jsonData, _token);
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
        private async Task<List<TopBanner>> GetList(int Id = 0)
        {
            List<TopBanner> list = new List<TopBanner>();
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/TopBanner/GetDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<TopBanner>>>(apiResponse.Result);
                list = deserializeObject.Result;
            }
            return list;
        }
    }
}
