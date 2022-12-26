using AppUtility.APIRequest;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode.Helper;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private string _apiBaseURL;
        private readonly ILogger<UserHomeController> _logger;
        private IGenericMethods _convert;

        public UserHomeController(ILogger<UserHomeController> logger, AppSettings appSettings, IGenericMethods convert)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _logger = logger;
            _convert = convert;
        }

        [Route("/dashboard")]
        public async Task<IActionResult> Index()
        {
            string path = "_AdminDashboard";
            //if (role.Equals("apiuser", StringComparison.OrdinalIgnoreCase) || role.Equals("2"))
            //{
            //    path = "_ApiDashboard";
            //    if (!await _upiSetting.IsAnyConfigurationExists(User.GetLoggedInUserId<int>()))
            //    {
            //        path = "~/views/Merchant/Index.cshtml";
            //    }
            //}
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult product()
        {
            return View();
        }

        [HttpGet("User/Profile")]
        public IActionResult Profile()
        {
            Role Roles = (Role)Enum.Parse(typeof(Role), User.GetLoggedInUserRoles());
            return View(Roles);
        }
        [HttpPost("User/ProfileDetails")]
        public async Task<IActionResult> ProfileDetails()
        {
            var res = await _convert.GetItem<UserDetails>("User/GetUserById", GetToken());
            return PartialView("PartailView/_ProfileDetails", res);
        }
        [HttpPost("User/EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var res = await _convert.GetItem<UserDetails>("User/GetUserById", GetToken());
            return PartialView("PartailView/_EditProfile", res);
        }
        [HttpPost("User/OrderList")]
        public async Task<IActionResult> OrderList()
        {
            var res = await _convert.GetList<OrderDetailsColumn>("OrderDetails/GetDetails", GetToken(), new OrderDetailsRequest());
            return PartialView("PartailView/_OrderList", res);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProfile(UserDetails model)
        {
            Response response = new Response();
            if(string.IsNullOrEmpty(model.PhoneNumber) || string.IsNullOrEmpty(model.PhoneNumber))
            {
                return Json(new Response { ResponseText = "All Fields Are Mandatory", StatusCode = ResponseStatus.Failed});
            }
            try
            {
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/User/SaveProfileInfo", JsonConvert.SerializeObject(model), GetToken());
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                    response = deserializeObject;
                }
            }
            catch (Exception ex)
            {
                response.ResponseText = ex.Message;
            }
            return Ok(response);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
