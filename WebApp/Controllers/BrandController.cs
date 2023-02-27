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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
using Service.Models;
using System;
using AppUtility.Helper;
using Entities.Enums;

namespace WebApp.Controllers
{


    [Authorize]
    public class BrandController : Controller
    {

        private string _apiBaseURL;
        private readonly IDDLHelper _ddl;
        public BrandController(ILogger<BrandController> logger, AppSettings appSettings, IDDLHelper ddl) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ddl = ddl;
        }

        // GET: BrandController
        [HttpGet("/Brand")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: BrandController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost("Brand/List")]
        public async Task<ActionResult> List(int Id)
        {
            List<Brands> brands = new List<Brands>();
            string _token = User.GetLoggedInUserToken();
            var brandRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/GetBrands", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (brandRes.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Brands>>>(brandRes.Result);
                brands = deserializeObject.Result;
            }
            return PartialView(brands);
        }
        [HttpPost("Brand/UpdateIsPublishBrand")]
        public async Task<IActionResult> UpdateIsPublishBrand(UpdateIspublishBrands req)
        {
            var res = new Response();
            if (req.Id >= 1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/UpdateIsPublishBrand", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(res);
        }

        // GET: BrandController/Create
        public async Task<ActionResult> Create(int Id = 0)
        {
            BrandVM brands = new BrandVM();
            if (Id != 0)
            {
                string _token = User.GetLoggedInUserToken();
                var brandRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/GetBrands", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
                if (brandRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<BrandVM>>>(brandRes.Result);
                    brands = deserializeObject.Result.FirstOrDefault();
                }
            }
            return PartialView(brands);
        }

        // POST: BrandController/Create
        [HttpPost]
        public async Task<ActionResult> Create(BrandVM brands)
        {
            var response = new Response();
            try
            {
                string _token = User.GetLoggedInUserToken();
                var body = JsonConvert.SerializeObject(new Brands { Id = brands.Id, Name = brands.Name, IsPublished = brands.IsPublished});
                var brandRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/AddUpdate", body, _token);
                if (brandRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(brandRes.Result);
                    response = deserializeObject;
                    if(response.StatusCode == ResponseStatus.Success)
                    {
                        string urrr = FileDirectories.BrandImages.Replace("//", "/");
                        var _ = Utility.O.UploadFile(new FileUploadModel
                        {
                            file = brands.Image,
                            FileName = response.ResponseText + ".jpeg",
                            FilePath = FileDirectories.BrandImages.Replace("//", "/")
                        });
                        response.ResponseText = response.StatusCode == ResponseStatus.Success ? "Success" : "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseText = ex.Message;
            }
            return Ok(response);

        }

        [HttpPost("Brand/BrandJSON")]
        public async Task<ActionResult> BrandJSON(int Id)
        {
            var brands = await _ddl.GetBrandsDDL(User.GetLoggedInUserToken(), _apiBaseURL);
            return Json(brands);
        }

        // GET: BrandController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BrandController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
