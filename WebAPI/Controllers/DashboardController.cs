using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Entities.Models;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboard;
        public DashboardController(IDashboard dashboard)
        {
            _dashboard = dashboard;
        }
        [Route("Dashboard/GetDashboardTopBoxCount")]
        public async Task<IActionResult> GetDashboardTopBoxCount() => Ok(await _dashboard.GetDashboardTopBoxCount(new Request { LoginId = User.GetLoggedInUserId<int>(), RoleId = Convert.ToInt32(User.GetLoggedInUserRoles()) }));
    }
}
