using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
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
        [Route("Report/GetInventoryReport")]
        public async Task<IActionResult> GetInventoryReport(InventoryRequest req) => Ok(await _report.GetInventoryReport(new RequestBase<InventoryRequest> { RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()), Data = req }));
    }
}
