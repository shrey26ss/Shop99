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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode.Attributes;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "1")]
    public class WebsiteinfoController : Controller
    {
        private string _apiBaseURL;
        public WebsiteinfoController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetwebsiteList()
        {
            List<WebsiteinfoModel> resList = await GetList();
            return PartialView("PartialView/_GetList", resList);
        }
        public async Task<IActionResult> Edit(int Id = 0)
        {
            var res = new WebsiteinfoViewModel();
            if (Id > 0)
            {
                List<WebsiteinfoModel> resList = await GetList(Id);
                res.Whitelogo = resList.FirstOrDefault().Whitelogo;
                res.Coloredlogo = resList.FirstOrDefault().Coloredlogo;
                res.Companydomain = resList.FirstOrDefault().Companydomain;
                res.Companyname = resList.FirstOrDefault().Companyname;
                res.CompanyemailID = resList.FirstOrDefault().CompanyemailID;
                res.Companymobile = resList.FirstOrDefault().Companymobile;
                res.Companyaddress = resList.FirstOrDefault().Companyaddress;
                res.Footerdescription = resList.FirstOrDefault().Footerdescription;
            }
            return PartialView("PartialView/_Addwebsiteinfo", res);
        }
        private async Task<List<WebsiteinfoModel>> GetList(int Id = 0)
        {
            List<WebsiteinfoModel> list = new List<WebsiteinfoModel>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Websiteinfo/GetDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<WebsiteinfoModel>>>(apiResponse.Result);
                list = deserializeObject.Result;
            }
            return list;
        }
        [HttpPost]
        public async Task<IActionResult> Save(WebsiteinfoViewModel model)
        {
            Response response = new Response();
            try
            {
                string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}.jpg";
                if (model.Whitelogofile != null)
                {
                    var _ = Utility.O.UploadFile(new FileUploadModel
                    {
                        file = model.Whitelogofile,
                        FileName = fileName,
                        FilePath = FileDirectories.Websiteinfo,
                    });
                    if (_.StatusCode == ResponseStatus.Success)
                        model.Whitelogo = $"{absoluteURL}/{FileDirectories.WebsiteinfoSuffix}/{fileName}";
                }
                if (model.Coloredlogofile != null)
                {
                    var _ = Utility.O.UploadFile(new FileUploadModel
                    {
                        file = model.Coloredlogofile,
                        FileName = fileName,
                        FilePath = FileDirectories.Websiteinfo,
                    });
                    if (_.StatusCode == ResponseStatus.Success)
                        model.Coloredlogo = $"{absoluteURL}/{FileDirectories.WebsiteinfoSuffix}/{fileName}";
                }
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Websiteinfo/AddUpdate", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
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
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = new Response();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Websiteinfo/Delete", JsonConvert.SerializeObject(new SearchItem { Id = Id }), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(response);
        }
    }
}
