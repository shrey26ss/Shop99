using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.API;
using Service.Identity;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
  
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/PlaceOrder")]
    public class PlaceOrderController : ControllerBase
    {
        private readonly IPlaceOrder _placeorder; 
        private string _alternateDomain;
        public PlaceOrderController(IPlaceOrder placeorder, PaymentServiceSetting payService)
        {
            _placeorder = placeorder;
            _alternateDomain = payService.AlternateDomain;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(GetPaymentMode))]
        public async Task<IActionResult> GetPaymentMode(bool IsCod)
        {
            return Ok(await _placeorder.GetPaymentMode(IsCod));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(PlaceOrder))]
        public async Task<IActionResult> PlaceOrder(PlaceOrderReq req)
        {
            req.AlternateDomain = _alternateDomain;
            return Ok(await _placeorder.PlaceOrder(new RequestBase<PlaceOrderReq>
            {
                Data = req, LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(ApplyCoupon))]
        public async Task<IActionResult> ApplyCoupon(CouponApplyRequest req)
        {
            req.UserID = User.GetLoggedInUserId<int>();
            return Ok(await _placeorder.ApplyCoupon(req));
        }
    }
}
