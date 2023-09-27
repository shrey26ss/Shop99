using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Servcie;

namespace WebApp.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {

        private readonly ICheckOutAPI _checkout;
        public CheckOutController(ICheckOutAPI checkout)
        {
            _checkout = checkout;
        }

        [HttpGet("CheckOut/{IsBuyNow?}")]
        public async Task<IActionResult> CheckOut(bool IsBuyNow = false)
        {
            return View(IsBuyNow);
        }

        [HttpPost(nameof(GetUserAddress))]
        public async Task<IActionResult> GetUserAddress()
        {
            var res = _checkout.GetUserGetAddress(GetToken()).Result;
            return PartialView("Partial/_address", res);
        }

        [HttpPost(nameof(GetPaymentMode))]
        public async Task<IActionResult> GetPaymentMode(bool IsCod)
        {
            var res = _checkout.GetPaymentMode(IsCod, GetToken()).Result;
            return Json(res);
        }

        [HttpPost(nameof(PlaceOrder))]
        public async Task<IActionResult> PlaceOrder(PlaceOrderReq req)
        {
            var res = await _checkout.PlaceOrder(req, GetToken());
            if (res != null && res.pgResponse != null && !string.IsNullOrEmpty(res.pgResponse.TID))
            {
                return PartialView("PGRedirect", res.pgResponse);
            }
            else
            {
                return Json(res);
            }
        }

        [HttpPost(nameof(SaveAddress))]
        public async Task<IActionResult> SaveAddress(UserAddress model)
        {
            if (ModelState.IsValid)
            {
                var res = _checkout.AddAddress(model, GetToken()).Result;
                return Json(res);
            }
            else
            {
                return Json(ModelState);
            }
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }

        [HttpPost(nameof(ApplyCoupon))]
        public async Task<IActionResult> ApplyCoupon(CouponApplyRequest req)
        {
            var res = await _checkout.ApplyCoupon(req, GetToken());
            return Json(res);
        }

        [HttpPost(nameof(GetAllCoupon))]
        public async Task<IActionResult> GetAllCoupon(int paymentmode = 1)
        {
            var res = await _checkout.GetAllCoupon(paymentmode, GetToken());
            return PartialView("Partial/_getallcoupons", res);
        }
    }
}