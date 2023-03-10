﻿using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Middleware;
using Service.CartWishList;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "1,3,4")]
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
        public async Task<IActionResult> GetInventoryLadgerReport(InventoryRequest req) => Ok(await _report.GetInventoryLadgerReport(new RequestBase<InventoryRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));
        [Route("Report/GetInventoryReport")]
        public async Task<IActionResult> GetInventoryReport(InventoryRequest req) => Ok(await _report.GetInventoryReport(new RequestBase<InventoryRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));
        [Route("Report/ReviewReport")]
        public async Task<IActionResult> ReviewReport(SearchItem req) => Ok(await _report.ReviewReport(req));

        [Route("Report/GetNewslatter")]
        public async Task<IActionResult> GetNewslatter() => Ok(await _report.GetNewslatter());

        [HttpPost("Report/GetPGReport")]
        public async Task<IActionResult> GetPGReport(InitiatePaymentRequest req) => Ok(await _report.GetPGReport(new RequestBase<InitiatePaymentRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));

        [HttpPost("Report/UpdateTransactionStatus")]
        public async Task<IActionResult> UpdateTransactionStatus(TransactionStatusRequest req) => Ok(await _pgService.UpadateTransactionStatus(new RequestBase<TransactionStatusRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));
    }
}
