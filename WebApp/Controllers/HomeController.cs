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
        private readonly ICategory _category;
        public HomeController(ILogger<UserHomeController> logger, ICategory category)
        {
            _logger = logger;
            _category = category;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("LoadMainCategory")]
        [HttpPost]
        public async Task<IActionResult> GetMainCategory()
        {
           // var res = _category.GetMenu();
            return Json("OK");
        }
    }
}
