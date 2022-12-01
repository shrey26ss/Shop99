using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Servcie;

namespace WebApp.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<UserHomeController> _logger;
        private readonly ICategoryAPI _category;
        public HomeController(ILogger<UserHomeController> logger, ICategoryAPI category)
        {
            _logger = logger;
            _category = category;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("LoadTopCategory")]
        [HttpPost]
        public async Task<IActionResult> GetTopCategory()
        {
             var res = _category.GetTopCategory().Result;
             return Json(res);
        }

        
      
        public async Task<IActionResult> Error(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("404");
            }
            return View("Error");
        }
    }
}
