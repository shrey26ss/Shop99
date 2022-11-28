using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "3")]
    public class VendorController : Controller
    {

        private string _apiBaseURL;
        public VendorController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        // GET: VendorController
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
                    //var role = User.GetLoggedInUserRoles();
                    //var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                    //identity.RemoveClaim(new Claim(ClaimTypes.Role, User.GetLoggedInUserRoles()));
                    //var role2 = User.GetLoggedInUserRoles();
                    return RedirectToAction("VendorDetails");
                }
            }
            return View(isValidvendor);
        }
        public IActionResult VendorDetails()
        {
            return View();
        }

        // GET: VendorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorController/Create
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VendorController/Delete/5
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
