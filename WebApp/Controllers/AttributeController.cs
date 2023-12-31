﻿using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{

    [Authorize]
    public class AttributeController : Controller
    {
        private string _apiBaseURL;

        public AttributeController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        [HttpGet("/Attribute")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: AttributeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost("Attribute/List")]
        public async Task<ActionResult> List()
        {
            List<Attributes> attributes = new List<Attributes>();
            string _token = User.GetLoggedInUserToken();
            var attributesRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/GetAttributes", JsonConvert.SerializeObject(new SearchItem()), _token);
            if (attributesRes.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Attributes>>>(attributesRes.Result);
                attributes = deserializeObject.Result;
            }
            return PartialView(attributes);
        }
        // GET: AttributeController/Create
        public async Task<ActionResult> Create(int Id)
        {
            Attributes attributes = new Attributes();
            if (Id != 0)
            {
                string _token = User.GetLoggedInUserToken();
                var attributesRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/GetAttributes", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
                if (attributesRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<List<Attributes>>>(attributesRes.Result);
                    attributes = deserializeObject.Result.FirstOrDefault();
                }
            }
            return PartialView(attributes);
        }

        // POST: AttributeController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Attributes attributes)
        {
            Response response =new Response();
            try
            {
                string _token = User.GetLoggedInUserToken();
                var jsonData = JsonConvert.SerializeObject(attributes);
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/AddUpdate", jsonData, _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                    response = deserializeObject;
                }
            }
            catch(Exception ex)
            {
                response.ResponseText = ex.Message;
            }
            return Ok(response);  
        }
       

        // GET: AttributeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttributeController/Delete/5
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
        [HttpPost]
        public async Task<IActionResult> UpdateIsPublishAttribute(UpdateIspublishAttr req)
        {
            var res = new Response();
            if (req.ID >= 1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Attribute/UpdateIsPublishAttr", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(res);
        }
    }
}
