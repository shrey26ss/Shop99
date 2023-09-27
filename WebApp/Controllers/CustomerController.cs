using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using WebApp.Models;
using Service.Identity;
using AppUtility.APIRequest;
using Entities.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using Service.Models;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    [Authorize(Roles = "ADMIN,CUSTOMER")]
    public class CustomerController : Controller
    {

        private string _apiBaseURL;
        private ILogger _logger;
        public CustomerController(ILogger<CustomerController> logger, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _logger = logger;
        }
        [Route("/Profile")]
        public async Task<IActionResult> Profile()
        {
            //var users = await _userManager.GetUserAsync(User);
            //if (users == null)
            //{
            //    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}
            return View();
        }
        [Route("/CustumerDetails")]
        public async Task<IActionResult> CustumerDetails()
        {
            return View();
        }
        public async Task<IActionResult> CustomerList()
        {
            List<UserDetails> list = new List<UserDetails>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/User/CustomerList",null, GetToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<UserDetails>>>(apiResponse.Result);
                list = deserializeObject.Result;
            }
            return PartialView("PartialView/_customerlist", list);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
