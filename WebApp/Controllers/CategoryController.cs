using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.API;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using WebApp.Middleware;
using System;
using Entities.Enums;
using AppUtility.Helper;
using WebApp.Models.ViewModels;
using Service.Models;
using WebApp.AppCode.Attributes;

namespace WebApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller

    {
        private string _apiBaseURL;
        private IDDLHelper _ddl;

        public CategoryController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings,IDDLHelper ddl) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ddl = ddl;
        }


        // GET: CategoryController
        [HttpGet("/Category")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return PartialView();
        }

        [HttpPost("Category/List")]
        public async Task<ActionResult> List(int Id)
        {
            List<Category> categories = new List<Category>();
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetCategory", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Category>>>(apiResponse.Result);
                categories = deserializeObject.Result;
            }
            return PartialView(categories);
        }

        // GET: CategoryController/Create
        public async Task<IActionResult> Create(int Id = 0)
        {
            var category = new CategoryViewModel();
            if (Id != 0)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetCategory", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<CategoryViewModel>>>(categoryrRes.Result);
                    category = deserializeObject.Result.FirstOrDefault();
                }
            }
            category.categoryDDLs = await _ddl.GetCategoryDDL(GetToken(), _apiBaseURL);
            return PartialView(category);
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAjax]
        public async Task<ActionResult> Create(Category category, IFormFile Icon)
        {
            Response response = new Response();
            if (Icon is null && category.CategoryId==0)
            {
                response.ResponseText = "Icon is must to upload";
                return Json(response);
            }
            try
            {
                string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}.svg";
                if (Icon != null)
                {
                    var _ = Utility.O.UploadFile(new FileUploadModel
                    {
                        file = Icon,
                        FileName = fileName,
                        FilePath = FileDirectories.Category,
                    });
                    if (_.StatusCode == ResponseStatus.Success)
                        category.Icon = $"{absoluteURL}/{FileDirectories.CategorySuffix}{fileName}";
                }
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/AddUpdate", JsonConvert.SerializeObject(category), User.GetLoggedInUserToken());
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    response = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }


        // GET: CategoryController/Delete/5

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
