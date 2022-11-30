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
  
    public class HomeController : Controller
    {
        private readonly ILogger<UserHomeController> _logger;

        public HomeController(ILogger<UserHomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
