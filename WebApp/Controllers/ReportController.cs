using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using WebApp.AppCode.Helper;
using WebApp.Models;
using WebApp.Servcie;
using WebApp.Middleware;
using Service.Models;
using Entities.Enums;

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
        public IActionResult _Inventory(InventoryStatus status = InventoryStatus.All)
        {
            return View(new InventoryRequest { Status = status });
        }
        [HttpPost("Report/InventoryList")]
        public async Task<IActionResult> InventoryList(InventoryRequest request)
        {
            var res = await _convert.GetList<Inventory>("Report/GetInventoryReport", GetToken(), request);
            return PartialView("Partial/_InventoryList", res);
        }
        [Route("Report/InventoryLedger")]
        public IActionResult Inventory(InventoryStatus status = InventoryStatus.All)
        {
            return View(new InventoryRequest {  Status = status});
        }
        [HttpPost("Report/InventoryLedgerList")]
        public async Task<IActionResult> InventoryLedgerList(InventoryRequest request)
        {
            var res = await _convert.GetList<Inventory>("Report/GetInventoryLadgerReport", GetToken(), request);
            return PartialView("Partial/InventoryList", res);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }

    }
}
