using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    
    [ApiController]
    [Route("/api/")]
    public class WebsiteinfoController : ControllerBase
    {
        private readonly IWebsiteinfo _websiteinfo;
        public WebsiteinfoController(IWebsiteinfo websiteinfo) => _websiteinfo = websiteinfo;
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Websiteinfo/AddUpdate")]
        public async Task<IActionResult> AddUpdate(WebsiteinfoModel req) => Ok(await _websiteinfo.AddUpdate(new RequestBase<WebsiteinfoModel>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));
        [HttpPost]
        [Route("Websiteinfo/GetDetails")]
        public async Task<IActionResult> GetDetails(SearchItem req) => Ok(await _websiteinfo.GetDetails(new RequestBase<SearchItem>
        {
            Data = req
        }));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Websiteinfo/Delete")]
        public async Task<IActionResult> Delete(SearchItem req) => Ok(await _websiteinfo.Delete(new RequestBase<SearchItem>
        {
            Data = req
        }));
    }
}
