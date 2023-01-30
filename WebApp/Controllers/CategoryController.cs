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

        public CategoryController(AppSettings appSettings,IDDLHelper ddl) //IRepository<EmailConfig> emailConfig, 
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
            List<Category> categories = await GetCategories(Id);
            return PartialView(categories);
        }

        [HttpPost("Category/CategoryJSON")]
        public async Task<ActionResult> CategoryJSON(int Id)
        {
            List<Category> categories = await GetCategories(Id);
            return Json(categories);
        }

        private async Task<List<Category>> GetCategories(int Id)
        {
            List<Category> categories = new List<Category>();
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetCategory", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Category>>>(apiResponse.Result);
                categories = deserializeObject.Result;
            }
            return categories;
        }

        [Authorize(Roles = "1")]
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

        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateIsPublishCategory(CategoryIsPublishUpdate req)
        {
            var res = new Response();
            if (req.ParentId >=1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/UpdateIsPublishCategory", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                  res= JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(res);
        }


        // POST: CategoryController/Create
        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateAjax]
        public async Task<ActionResult> Create(Category category, IFormFile Icon)
        {
            Response response = new Response();
            try
            {
                if (Icon != null)
                {
                string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    string extention=System.IO.Path.GetExtension(Icon.FileName);
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}"+extention;
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
