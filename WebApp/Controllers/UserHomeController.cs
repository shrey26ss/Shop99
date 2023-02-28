using AppUtility.APIRequest;
using AppUtility.Helper;
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
using WebApp.AppCode;
using WebApp.AppCode.Helper;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Servcie;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private string _apiBaseURL;
        private readonly ILogger<UserHomeController> _logger;
        private IGenericMethods _convert;
        private readonly ICheckOutAPI _checkout;
        private readonly IDDLHelper _ddl;
        private readonly IHttpRequestInfo _httpInfo;

        public UserHomeController(ILogger<UserHomeController> logger, AppSettings appSettings, IGenericMethods convert, ICheckOutAPI checkOutAPI, IDDLHelper ddl, IHttpRequestInfo httpInfo)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _logger = logger;
            _convert = convert;
            _checkout = checkOutAPI;
            _ddl = ddl;
            _httpInfo = httpInfo;
        }
        [Authorize(Roles = "1")]
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
            Profileviewmodel model = new Profileviewmodel();
            model.Role = (Role)Enum.Parse(typeof(Role), User.GetLoggedInUserRoles());
            model.loginId = User.GetLoggedInUserId<int>();
            model.profilepic = _httpInfo.AbsoluteURL() + "/" + FileDirectories.UserpicSuffix + User.GetLoggedInUserId<int>() + ".jpeg";
            return View(model);
        }
        [HttpPost("User/ProfileDetails")]
        public async Task<IActionResult> ProfileDetails()
        {
            var res = await _convert.GetItem<UserDetails>("User/GetUserById", GetToken());
            return PartialView("PartailView/_ProfileDetails", res ?? new UserDetails());
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
        [HttpPost("User/UserAddress")]
        public async Task<IActionResult> UserAddress()
        {
            var res = _checkout.GetUserGetAddress(GetToken()).Result;
            return PartialView("PartailView/_UserAddress", res.Result ?? new List<UserAddress>());
        }
        [HttpPost("User/EditAddress")]
        public async Task<IActionResult> EditAddress(int Id = 0)
        {
            var _ = _checkout.GetUserGetAddress(GetToken()).Result;
            var res = _.Result.ToList().Where(a => a.Id.Equals(Id)).FirstOrDefault() ?? new UserAddress();
            res.stateDDLs = await _ddl.GetStateDDL(_apiBaseURL);
            return PartialView("PartailView/_EditAddress", res);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProfile(UserDetails model)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email))
            {
                return Json(new Response { ResponseText = "All Fields Are Mandatory", StatusCode = ResponseStatus.Failed });
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
        [HttpPost("T-B-C")]
        public async Task<IActionResult> TopBoxCounts()
        {
            return Ok(await _convert.GetItem<DashboardTopBoxCount>(@"Dashboard/GetDashboardTopBoxCount", GetToken()));
        }
        [HttpPost("G-O-L")]
        public async Task<IActionResult> OrdersList()
        {
            var res = await _convert.GetList<OrderDetailsColumn>("OrderDetails/GetDetails", GetToken(), new OrderDetailsRequest
            {
                Id = 0,
                StatusID = 0,
                Top = 5
            });
            return Json(res);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
