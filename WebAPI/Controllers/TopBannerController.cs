﻿using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "1,3,4")]
    [ApiController]
    [Route("/api/")]
    public class TopBannerController : ControllerBase
    {
        private readonly ITopBanner _banner;

        public TopBannerController(ITopBanner banner) => _banner = banner;
        [Route("TopBanner/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(TopBanner req) => Ok(await _banner.AddUpdate(new RequestBase<TopBanner>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));
        [Route("TopBanner/GetDetails")]
        public async Task<IActionResult> GetDetails(SearchItem req) => Ok(await _banner.GetDetails(new RequestBase<SearchItem>
        {
            Data = req
        }));
        [Route("TopBanner/Delete")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(SearchItem req) => Ok(await _banner.Delete(new RequestBase<SearchItem>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));
    }
}
