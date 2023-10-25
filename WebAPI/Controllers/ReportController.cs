using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Middleware;
using Service.CartWishList;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN,VENDOR,DEVELOPER")]
    [ApiController]
    [Route("/api/")]
    public class ReportController : ControllerBase
    {
        private readonly IReport _report;
        private readonly IPGCallback _pgService;
        public ReportController(IReport report, IPGCallback pgService)
        {
            _report = report;
            _pgService = pgService;
        }
        [Route("Report/GetInventoryLadgerReport")]
        public async Task<IActionResult> GetInventoryLadgerReport(InventoryRequest req) => Ok(await _report.GetInventoryLadgerReport(new RequestBase<InventoryRequest> { RoleId = User.GetLoggedInUserRole<int>(), Data = req }));
        [Route("Report/GetInventoryReport")]
        public async Task<IActionResult> GetInventoryReport(InventoryRequest req) => Ok(await _report.GetInventoryReport(new RequestBase<InventoryRequest> { RoleId = User.GetLoggedInUserRole<int>(), Data = req }));

        [Route("Report/UserWalletLedgerList")]
        public async Task<IActionResult> UserWalletLedgerList(UserWalletLedgerRequest request)
        {
             request.UserID = User.GetLoggedInUserId<int>();
            var result = await _report.GetUserWalletLedger(request);
            return Ok(result);
        }
        [Route("Report/ReviewReport")]
        public async Task<IActionResult> ReviewReport(SearchItem req) => Ok(await _report.ReviewReport(req));

        [Route("Report/GetNewslatter")]
        public async Task<IActionResult> GetNewslatter() => Ok(await _report.GetNewslatter());

        [Route("Report/GetTransactionReqRes/{TID}")]
        public async Task<IActionResult> GetTransactionReqRes(string TID)
        {
            
            var result = await _report.GetTransactionReqRes(TID);
            return Ok(result);
        }


        [HttpPost("Report/GetPGReport")]
        public async Task<IActionResult> GetPGReport(InitiatePaymentRequest req) => Ok(await _report.GetPGReport(new RequestBase<InitiatePaymentRequest> { RoleId = User.GetLoggedInUserRole<int>(),Data= req }));

        [HttpPost("Report/UpdateTransactionStatus")]
        public async Task<IActionResult> UpdateTransactionStatus(TransactionStatusRequest req) => Ok(await _pgService.UpadateTransactionStatus(new RequestBase<TransactionStatusRequest> { RoleId = User.GetLoggedInUserRole<int>(), Data = req }));
        [HttpPost("Report/TransactionStatuscheck")]
        public async Task<IActionResult> TransactionStatuscheck(TransactionStatusRequest req) => Ok(await _pgService.TransactionStatuscheck(new RequestBase<TransactionStatusRequest> { RoleId = User.GetLoggedInUserRole<int>(), Data = req }));
    }
}
