using AppUtility.APIRequest;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.AppCode.Attributes;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class VendorController : Controller
    {

        private string _apiBaseURL;
        private readonly SignInManager<Service.Identity.ApplicationUser> _signInManager;
        public VendorController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings, SignInManager<Service.Identity.ApplicationUser> signInManager)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _signInManager = signInManager;
        }
        // GET: VendorController
        [Authorize(Roles = "0,3")]
        public async Task<IActionResult> Index()
        {
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Vendor/ValidateVendor", "", _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<bool>>(apiResponse.Result);
                if (deserializeObject.Result == true)
                {
                    var Identity = HttpContext.User.Identity as ClaimsIdentity;
                    Identity.RemoveClaim(Identity.FindFirst(ClaimTypes.Role));
                    Identity.AddClaim(new Claim(ClaimTypes.Role, "3"));
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(Identity));
                    return View();
                }
            }
            return RedirectToAction(nameof(Onboard));
        }
        [Authorize(Roles ="0")]
        public IActionResult Onboard()
        {
            return View();
        }
        [Authorize(Roles ="0")]
        [HttpPost]
        public async Task<IActionResult> VendorDetails(VendorProfile model)
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
                            Identity.AddClaim(new Claim(ClaimTypes.Role, "3"));
                            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(Identity));
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.ResponseText = ex.Message;
                }
                return RedirectToAction("Index");
            }
            return View(model);
            
        }
        // GET: VendorController/Create
        [Authorize(Roles = "3")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorController/Create
        [Authorize(Roles = "3")]
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
        [Authorize(Roles = "3")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VendorController/Delete/5
        [Authorize(Roles = "3")]
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
