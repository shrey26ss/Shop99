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

namespace WebApp.Controllers
{
    [Authorize(Roles = "2")]
    public class CustomerController : Controller
    {

        private string _apiBaseURL;
        private ILogger _logger;
        public CustomerController(ILogger<CustomerController> logger, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _logger = logger;
        }
        public async Task<IActionResult> Profile()
        {
            //var users = await _userManager.GetUserAsync(User);
            //if (users == null)
            //{
            //    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            //}
            return View();
        }
    }
}
