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
using AppUtility.APIRequest;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize(Roles = "1")]
    public class ReportController : Controller
    {
        private string _apiBaseURL;
        private IGenericMethods _convert;

        public ReportController(ILogger<UserHomeController> logger, AppSettings appSettings, IGenericMethods convert)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _convert = convert;
        }
        public IActionResult _Inventory(StatusType status = StatusType.All)
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
        public IActionResult Inventory(StatusType status = StatusType.All)
        {
            return View(new InventoryRequest {  Status = status});
        }
        [HttpPost("Report/InventoryLedgerList")]
        public async Task<IActionResult> InventoryLedgerList(InventoryRequest request)
        {
            var res = await _convert.GetList<Inventory>("Report/GetInventoryLadgerReport", GetToken(), request);
            return PartialView("Partial/InventoryList", res);
        }
        [HttpPost("Report/ReviewReport")]
        public async Task<IActionResult> ReviewReport(SearchItem req)
        {
            var res = await _convert.GetList<ProductRatingReq>("Report/ReviewReport", GetToken(), req);
            return PartialView("Partial/ReviewReport", res);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
        [Route("Report/NewsLetter")]
        public async Task<IActionResult> NewsLetter()
        {
            string _token = GetToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Report/GetNewslatter", null, _token);
            if (apiResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var res = JsonConvert.DeserializeObject<NewsLetterResponse>(apiResponse.Result);
                return View(res);
            }
            return View();
        }

        [HttpGet("Report/PGReport")]
        public async Task<IActionResult> PGReport()
        {
            return View();
        }


        [HttpPost("Report/_PGReport")]
        public async Task<IActionResult> _PGReport(InitiatePaymentRequest request)
        {
            var res = await _convert.GetList<InitiatePayment>("Report/GetPGReport", GetToken(), request);
            return PartialView("Partial/_PGReport", res);
        }

        [HttpPost("Report/UpdateTransactionStatus")]
        public async Task<IActionResult> UpdateTransactionStatus(TransactionStatusRequest request)
        {
            var res = await _convert.GetAsync<Response>("Report/UpdateTransactionStatus", GetToken(), request);
            return Json(res);
        }
    }
}
