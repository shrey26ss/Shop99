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

        [HttpPost("GetUserAddress")]
        public async Task<IActionResult> GetUserAddress()
        {
            var res = _checkout.GetUserGetAddress(GetToken()).Result;
            return PartialView("Partial/_address", res);
        }

        [HttpPost("GetPaymentMode")]
        public async Task<IActionResult> GetPaymentMode(bool IsCod)
        {
            var res = _checkout.GetPaymentMode(IsCod, GetToken()).Result;
            return Json(res);
        }

        [HttpPost("PlaceOrder")]
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

        [HttpPost("SaveAddress")]
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
    }
}
