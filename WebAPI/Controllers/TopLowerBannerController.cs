using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class TopLowerBannerController : ControllerBase
    {
        private readonly ITopLowerBanner _banner;

        public TopLowerBannerController(ITopLowerBanner banner)
        {
            _banner = banner;
        }
        [Route("TopLowerBanner/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(TopLowerBanner req)
        {
            return Ok(await _banner.AddUpdate(new RequestBase<TopLowerBanner>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("TopLowerBanner/GetDetails")]
        public async Task<IActionResult> GetDetails()
        {
            return Ok(await _banner.GetDetails(new RequestBase<SearchItem>
            {
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
    }
}
