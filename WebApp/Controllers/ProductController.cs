using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private string _apiBaseURL;
        public ProductController(AppSettings appSettings) //IRepository<EmailConfig> emailConfig, _emailConfig = emailConfig;
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        // GET: ProductController
        [HttpGet("/Product")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductList()
        {
            var response = new List<Products>();
            string _token = User.GetLoggedInUserToken();
            var jsonData = JsonConvert.SerializeObject(new SearchItem { Id = 0 });
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/GetProducts", jsonData, _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Products>>>(apiResponse.Result);
                response = deserializeObject.Result;
            }
            return PartialView("Partials/_ProductList", response);
        }

        // GET: ProductController/Create
        public ActionResult Create(int Id = 0)
        {
            VariantViewModel model = new VariantViewModel()
            {
                ProductId= Id,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetProductSectionView(int Id = 0)
        {
            Products model = new Products();
            return PartialView("Partials/_Product",model);
        }
       
        // POST: ProductController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Products model)
        {
            var response = new Response();
            try
            {
                string _token = User.GetLoggedInUserToken();
                var jsonData = JsonConvert.SerializeObject(model);
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/AddUpdate", jsonData, _token);
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                    response = deserializeObject;
                }
            }
            catch
            {
                
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAttributeSectionView(int Id = 0)
        {
            return PartialView("Partials/_Variants");
        }
        [HttpPost]
        public async Task<IActionResult> AddAttributes()
        {
            return PartialView("Partials/_AddAttributes");
        }
        [HttpPost]
        public async Task<IActionResult> AddVariant(VariantCombination model)
        {
            var response = new Response();
            try
            {
                string _token = User.GetLoggedInUserToken();
                var jsonData = JsonConvert.SerializeObject(model);
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Product/AddProductVariant", jsonData, _token);
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                    response = deserializeObject;
                }
            }
            catch
            {

            }
            return Json(model);
        }

    }
}
