using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using WebApp.AppCode.Helper;
using WebApp.Models;
using WebApp.Servcie;
using WebApp.Middleware;

namespace WebApp.Controllers
{
    public class ReportController : Controller
    {
        private string _apiBaseURL;
        private IGenericMethods _convert;

        public ReportController(ILogger<UserHomeController> logger, AppSettings appSettings, IGenericMethods convert)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _convert = convert;
        }
        public IActionResult Inventory()
        {
            return View();
        }
        [HttpPost("Report/InventoryList")]
        public async Task<IActionResult> InventoryList()
        {
            var res = await _convert.GetList<Inventory>("Report/GetInventoryReport", GetToken());
            return PartialView("Partial/InventoryList", res);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }

    }
}
