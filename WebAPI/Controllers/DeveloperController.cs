using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloper _develop;

        public DeveloperController(IDeveloper develop) => _develop = develop;
        [HttpPost]
        [Route("Developer/GetList")]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _develop.GetImgList(User.GetLoggedInUserRoles()));
        }

    }
}
