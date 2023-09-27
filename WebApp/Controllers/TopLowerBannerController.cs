using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
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
    [Authorize(Roles = "ADMIN")]
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
        public async Task<IActionResult> Edit(TopLowerBannerViewModel model) //TopLowerBanner
        {
            Response response = new Response();
            try
            {
                if(model.Id == 0 && model.File == null)
                {
                    return Json(new Response { ResponseText = "Please Select a Image"});
                }
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}.jpg";
                if(model.File != null)
                {
                    var _ = Utility.O.UploadFile(new FileUploadModel
                    {
                        file = model.File,
                        FileName = fileName,
                        FilePath = FileDirectories.TopLowerBanner,
                    });
                    if (_.StatusCode == ResponseStatus.Success){
                        string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                        model.BannerPath = $"{absoluteURL}/{FileDirectories.TopLowerBannerSuffix}/{fileName}";
                    }
                }
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/TopLowerBanner/AddUpdate", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                    response = deserializeObject;
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
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = new Response();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/TopLowerBanner/Delete", JsonConvert.SerializeObject(new SearchItem { Id = Id }), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(response);
        }
    }
}
