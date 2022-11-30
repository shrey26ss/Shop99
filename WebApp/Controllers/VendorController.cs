using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApp.Middleware;
using WebApp.Models;
using static Dapper.SqlMapper;

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
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Index()
        {
            bool isValidvendor = true;
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Vendor/ValidateVendor", "", _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<bool>>(apiResponse.Result);
                if (deserializeObject.Result == false)
                {
                    isValidvendor = false;
                    var Identity = HttpContext.User.Identity as ClaimsIdentity;
                    Identity.RemoveClaim(Identity.FindFirst(ClaimTypes.Role));
                    Identity.AddClaim(new Claim(ClaimTypes.Role, "0"));
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(Identity));
                }
            }
            return View(isValidvendor);
        }
        [Authorize(Roles ="0")]
        public IActionResult VendorDetails()
        {
            return PartialView("PartialView/_VendorDetails");
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
