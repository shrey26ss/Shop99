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
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController] 
    [Route("/api/")]
    public class OffersController : ControllerBase
    {
        private readonly IOffersService _offers;
       public OffersController(IOffersService offers)
        {
            _offers = offers; 
        }
        [HttpPost]
        [Route("Offers/GetOffers")]
        public async Task<IActionResult> GetOffers(SearchItem req)
        {
            return Ok(await _offers.Getoffers(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("Offers/GetOffertypeList")]
        public async Task<IActionResult> GetOfferDDL()
        {
            return Ok(await _offers.GetOfferDDL());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("Offers/OfferAddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateOffer(GetOffers req)
        {
            return Ok(await _offers.AddUpdateOffer(new RequestBase<GetOffers>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("Offers/UpdateIsActiveOffer")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateIsActiveOffer(OfferUpdateIsActive req)
        {
            return Ok(await _offers.UpdateIsActiveOffer(new RequestBase<OfferUpdateIsActive>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Offers/GetCoupons")]
        public async Task<IActionResult> GetCoupons(SearchItem req)
        {
            return Ok(await _offers.GetCoupons(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [HttpPost]
        [Route("Offers/AddUpdateCoupon")]
        public async Task<IActionResult> AddUpdateCoupon(Coupon coupon)
        {
            return Ok( await _offers.AddUpdateCoupon(new RequestBase<Coupon>
            {
                Data = coupon,
                LoginId= User.GetLoggedInUserId<int>()
            }));
        }
        [HttpPost]
        [Route("Offers/DelCoupon")]
        public async Task<IActionResult> DelCoupon(Coupon cpn)
        {
            return Ok(await _offers.DelCoupon(new RequestBase<Coupon>
            {
                Data = cpn,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        
        [AllowAnonymous]
        [HttpPost("Offers/GetCartProductCoupons")]
        public async Task<IActionResult> GetCartProductCoupons()
        {
            return Ok(await _offers.GetCartProductCoupons(new RequestBase<SearchItem>
            {
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
    }
}
