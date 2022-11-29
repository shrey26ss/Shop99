using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    [Authorize]
    public class ProductController : Controller
    {

        private string _apiBaseURL;
        public ProductController(AppSettings appSettings) //IRepository<EmailConfig> emailConfig, _emailConfig = emailConfig;
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }

        #region Add Product
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

        [HttpPost]
        public async Task<IActionResult> GetProductSectionView(int Id = 0)
        {
            var model = new ProductViewModel();
            model.Categories = await DDLHelper.O.GetCategoryDDL(GetToken(), _apiBaseURL);
            model.Brands = await DDLHelper.O.GetBrandsDDL(GetToken(), _apiBaseURL);
            return PartialView("Partials/_Product", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Products model)
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
        #endregion


        #region Add Variants

        // GET: ProductController/Create
        public async Task<IActionResult> AddVariant(int Id = 0)
        {
            VariantViewModel model = new VariantViewModel()
            {

                ProductId = Id,
                AttributesDDLs = await DDLHelper.O.GetAttributeDDL(GetToken(), _apiBaseURL)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAttributeSectionView(int Id = 0)
        {
            return PartialView("Partials/_Variants");
        }

        public async Task<IActionResult> AddAttributeGroup()
        {
            return PartialView("Partials/_AddAttributeGroup");
        }

        [HttpPost]
        public async Task<IActionResult> AddAttributes(string combinationId)
        {
            var model = new ViewVariantCombinationModel
            {
                CombinationId = combinationId,
                Attributes  = await DDLHelper.O.GetAttributeDDL(GetToken(), _apiBaseURL)
            };
            return PartialView("Partials/_AddAttributes", model);
        }
        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> SaveVariants(List<PictureInformationReq> req, string jsonObj)
        {
            List<PictureInformation> ImageInfo = new List<PictureInformation>();
            if (req!=null && req.Any())
            {
                foreach (var item in req)
                {
                    string fileName = DateTime.Now.ToString("ddMMyyyyhhmmssmmm");
                    Utility.O.UploadFile(new FileUploadModel
                    {
                        file = item.file,
                        FileName = fileName,
                        FilePath = FileDirectories.ProductVariant,
                        IsThumbnailRequired = true,
                    });
                    ImageInfo.Add(new PictureInformation
                    {
                        Title = item.Title,
                        Alt = item.Alt,
                        Color = item.Color,
                        DisplayOrder = item.DisplayOrder,
                        GroupId = item.GroupId,
                        ImagePath = string.Concat("hhtps://yourdomain.com/", FileDirectories.ProductSuffix, fileName)
                    });
                }
            }
            var model = JsonConvert.DeserializeObject<VariantCombination>(jsonObj);
            model.PictureInfo=ImageInfo;
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
        #endregion


        #region Private Method
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
        #endregion

    }
}
