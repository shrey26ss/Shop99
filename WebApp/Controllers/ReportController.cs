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
using Infrastructure.Interface;
using PaymentGateWay.PaymentGateway.PayU;
using System.Net;
using WebApp.AppCode;

namespace WebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ReportController : Controller
    {
        private string _apiBaseURL;
        private IGenericMethods _convert;
        private readonly IHttpRequestInfo _httpInfo;

        public ReportController(ILogger<UserHomeController> logger, AppSettings appSettings, IGenericMethods convert, IHttpRequestInfo httpInfo)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _convert = convert;
            _httpInfo = httpInfo;
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
            return View(new InventoryRequest { Status = status });
        }
        [HttpPost("Report/InventoryLedgerList")]
        public async Task<IActionResult> InventoryLedgerList(InventoryRequest request)
        {
            var res = await _convert.GetList<Inventory>("Report/GetInventoryLadgerReport", GetToken(), request);
            return PartialView("Partial/InventoryList", res);
        }
        [Route("Report/UserWalletLedger")]
        public async Task<IActionResult> UserWalletLedger(StatusType status = StatusType.All)
        {
            return View(new UserWalletledgerRequest { Status = status });
        }
        [HttpPost("Report/UserWalletLedgerList")]
        public async Task<IActionResult> UserWalletLedgerList(string Phonenumber,int UserID)
        {
            var res = await _convert.GetList<UserWalletledger>("Report/UserWalletLedgerList", GetToken(), new {Phonenumber,UserID});
            return PartialView("Partial/_UserWalletLedger", res);
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

        [HttpPost("Report/GetTransactionReqRes")]
        public async Task<IActionResult> GetTransactionReqRes(string TID)
        {
            try
            {
                string _token = GetToken();
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Report/GetTransactionReqRes/{TID}", TID, _token);

                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var responseContent = JsonConvert.DeserializeObject<APIModelResponse>(apiResponse.Result);
                    return PartialView(responseContent);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return Ok("Error");
            }
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
        [HttpPost("Report/TransactionStatuscheck")]
        public async Task<IActionResult> TransactionStatuscheck(TransactionStatusRequest request)
        {
            var res = await _convert.GetAsync<Response>("Report/TransactionStatuscheck", GetToken(), request);
            return Json(res);
        }
    }
}
