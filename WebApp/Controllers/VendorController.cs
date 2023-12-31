﻿using AppUtility.APIRequest;
using AppUtility.Helper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.AppCode.Helper;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class VendorController : Controller
    {

        private string _apiBaseURL;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IDDLHelper _ddl;
        private readonly IGenericMethods _convert;
        public VendorController(AppSettings appSettings, SignInManager<ApplicationUser> signInManager,IDDLHelper ddl, IGenericMethods convert)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _signInManager = signInManager;
            _ddl = ddl;
            _convert = convert;
        }

        // GET: VendorController
        [Authorize(Roles = "0,3")]
        public async Task<IActionResult> Index()
        {
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Vendor/ValidateVendor", "", _token);
            bool IsApproved = false, IsOnboard = false;
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<ValidateVendor>>(apiResponse.Result);
                IsApproved = deserializeObject.Result.IsApproved;
                IsOnboard = deserializeObject.Result.IsOnboard;
                if (deserializeObject.Result.IsOnboard == true && deserializeObject.Result.IsApproved ==  true)
                {
                    var Identity = HttpContext.User.Identity as ClaimsIdentity;
                    Identity.RemoveClaim(Identity.FindFirst(ClaimTypes.Role));
                    Identity.AddClaim(new Claim(ClaimTypes.Role, "3"));
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(Identity));
                    return View();
                }
                else if (deserializeObject.Result.IsOnboard == true && deserializeObject.Result.IsApproved == false)
                {
                    return RedirectToAction(nameof(Onboard), "Vendor", new { IsApproved, IsOnboard });
                }
            }
            return RedirectToAction(nameof(Onboard), "Vendor", new { IsApproved, IsOnboard });
        }
        [Authorize]
        public async Task<IActionResult> Onboard(bool IsApproved = false, bool IsOnboard = false)
        {
            var model = new VendorVM
            {
                States = await _ddl.GetStateDDL(_apiBaseURL),
                IsApproved = IsApproved,
                IsOnboard = IsOnboard
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Onboard(VendorVM model)
        {
            if (ModelState.IsValid)
            {
                var response = new Response();
                try
                {
                    string _token = User.GetLoggedInUserToken();
                    var body = JsonConvert.SerializeObject(model);
                    var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Vendor/AddUpdate", body, _token);
                    if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                        response = deserializeObject;
                        if (response.StatusCode == ResponseStatus.Success)
                        {
                            var Identity = HttpContext.User.Identity as ClaimsIdentity;
                            Identity.RemoveClaim(Identity.FindFirst(ClaimTypes.Role));
                            Identity.AddClaim(new Claim(ClaimTypes.Role, "0"));
                            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(Identity));
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.ResponseText = ex.Message;
                }
                return RedirectToAction("Onboard", new {IsApproved = false, IsOnBoard = true});
            }
            model.States = await _ddl.GetStateDDL(_apiBaseURL);
            return View(model);
            
        }
        // GET: VendorController/Create
        [Authorize(Roles = "VENDOR")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorController/Create
        [Authorize(Roles = "VENDOR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: VendorController/Delete/5
        [Authorize(Roles = "VENDOR")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VendorController/Delete/5
        [Authorize(Roles = "VENDOR")]
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
        [Authorize(Roles ="ADMIN")]
        [HttpGet]
        [Route("/VendorList")]
        public async Task<IActionResult> Vendors()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult>VendorList(VendorProfileRequest model)
        {
            model = model ?? new VendorProfileRequest();
            var list = new List<VendorProfileList>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/User/VendorList", JsonConvert.SerializeObject
                (model), GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<VendorProfileList>>>(apiResponse.Result);
                list = deserializeObject.Result;
            }
            return PartialView("PartialView/_vendorList", list);
        }
        [Authorize(Roles ="ADMIN")]
        [HttpPost]
        public async Task<IActionResult> ApproveVendorProfile(int VendorId)
        {
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/User/ApproveVendorProfile", JsonConvert.SerializeObject
                (new { VendorId }), GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                return Json(deserializeObject);
            }
            return Json(new Response());
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
