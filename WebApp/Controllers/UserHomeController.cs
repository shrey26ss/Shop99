using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private readonly ILogger<UserHomeController> _logger;

        public UserHomeController(ILogger<UserHomeController> logger)
        {
            _logger = logger;
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
       
    }
}
