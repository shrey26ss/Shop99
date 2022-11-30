using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class TopBannerController : ControllerBase
    {
        private readonly ITopBanner _banner;

        public TopBannerController(ITopBanner banner)
        {
            _banner = banner;
        }
        [Route("TopBanner/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(TopBanner req)
        {
            return Ok(await _banner.AddUpdate(new RequestBase<TopBanner>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("TopBanner/GetDetails")]
        public async Task<IActionResult> GetDetails()
        {
            return Ok(await _banner.GetDetails(new RequestBase<SearchItem>
            {
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
    }
}
