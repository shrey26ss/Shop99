using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class ReportController : ControllerBase
    {
        private readonly IReport _report;
        public ReportController(IReport report)
        {
            _report = report;
        }
        [Route("Report/GetInventoryLadgerReport")]
        public async Task<IActionResult> GetInventoryLadgerReport(InventoryRequest req) => Ok(await _report.GetInventoryLadgerReport(new RequestBase<InventoryRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));
        [Route("Report/GetInventoryReport")]
        public async Task<IActionResult> GetInventoryReport(InventoryRequest req) => Ok(await _report.GetInventoryReport(new RequestBase<InventoryRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));
        [Route("Report/ReviewReport")]
        public async Task<IActionResult> ReviewReport(SearchItem req) => Ok(await _report.ReviewReport(req));

    }
}
