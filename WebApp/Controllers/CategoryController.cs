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
            return View();
        }

        // GET: CategoryController/Create
        public async Task<IActionResult> Create(int Id =0)
        {
            Category category = new Category();
            if (Id != 0)
            {
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/GetCategory", JsonConvert.SerializeObject(new SearchItem {Id=Id } ));
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<Category>>>(categoryrRes.Result);
                    category = deserializeObject.Result.FirstOrDefault();
                }
            }
            return View(category);
        }

        // POST: CategoryController/Create
        [HttpPost]

        public async Task<ActionResult> Create(Category category,IFormFile Icon)
        {
            Response response= new Response();
            try
            {
                var body = JsonConvert.SerializeObject(category);
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Category/AddUpdate", body);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                    response = deserializeObject;
                }
            }
            catch
            {
                return View();
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
