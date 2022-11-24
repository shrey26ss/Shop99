﻿using AppUtility.APIRequest;
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

namespace WebApp.Controllers
{


    [Authorize]
    public class BrandController : Controller
    {

        private string _apiBaseURL;
        public BrandController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }

        // GET: BrandController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BrandController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandController/Create
        public async Task<ActionResult> Create( int Id=0)
        {
            Brands brand = new Brands();
            if (Id != 0)
            {
                string _token = User.GetLoggedInUserToken();
                var brandRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/GetBrands", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
                if (brandRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<Brands>>>(brandRes.Result);
                    brand = deserializeObject.Result.FirstOrDefault();
                }
            }
            return View(brand);
        }

        // POST: BrandController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Brands brands, IFormFile Icon)
        {
            Response response = new Response();
            try
            {
                string _token = User.GetLoggedInUserToken();
                var body = JsonConvert.SerializeObject(brands);
                var brandRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Brand/AddUpdate", body, _token);
                if (brandRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(brandRes.Result);
                    response = deserializeObject;
                }
            }
            catch
            {
                return View();
            }
            return Json(response);

        }

        // GET: BrandController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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