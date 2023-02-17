using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("/api/")]
    public class WebsiteinfoController : ControllerBase
    {
        private readonly IWebsiteinfo _websiteinfo;
        public WebsiteinfoController(IWebsiteinfo websiteinfo) => _websiteinfo = websiteinfo;
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Websiteinfo/AddUpdate")]
        public async Task<IActionResult> AddUpdate(WebsiteinfoModel req) => Ok(await _websiteinfo.AddUpdate(new RequestBase<WebsiteinfoModel>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));
        [Route("Websiteinfo/GetDetails")]
        public async Task<IActionResult> GetDetails(SearchItem req) => Ok(await _websiteinfo.GetDetails(new RequestBase<SearchItem>
        {
            Data = req
        }));
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Websiteinfo/Delete")]
        public async Task<IActionResult> Delete(SearchItem req) => Ok(await _websiteinfo.Delete(new RequestBase<SearchItem>
        {
            Data = req
        }));
    }
}
