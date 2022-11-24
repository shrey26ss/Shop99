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
using AppUtility.APIRequest;
using System.Collections.Generic;
using System.Linq;
using WebApp.Middleware;
using System;
using Entities.Enums;
using AppUtility.Helper;

namespace WebApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller

    {
        private string _apiBaseURL;

        public CategoryController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
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
        public async  Task<ActionResult> List(int Id)
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
            Category category = new Category();
            if (Id != 0)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetCategory", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<Category>>>(categoryrRes.Result);
                    category = deserializeObject.Result.FirstOrDefault();
                }
            }
            return PartialView(category);
        }

        // POST: CategoryController/Create
        [HttpPost]

        public async Task<ActionResult> Create(Category category, IFormFile Icon)
        {
            Response response = new Response();
            try
            {
                string fileName = $"{DateTime.Now.ToString("ddmmyyhhssmmttt")}.svg";
                var _ = Utility.O.UploadFile(new FileUploadModel
                {
                    file = Icon,
                    FileName = fileName,
                    FilePath = FileDirectories.Category,
                });
                if(_.StatusCode== ResponseStatus.Success)
                {
                    string _token = User.GetLoggedInUserToken();
                    string absoluteURL = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    category.Icon = $"{absoluteURL}/{FileDirectories.CategorySuffix}/{fileName}";
                    var jsonData = JsonConvert.SerializeObject(category);
                    var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/AddUpdate", jsonData, _token);
                    if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                        response = deserializeObject;
                    }
                }
            }
            catch(Exception ex)
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
    }
}
