using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Servcie;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartWishListController : Controller
    {
        private readonly ICartWishListAPI _cartwishlist;
        private readonly ICheckOutAPI _checkout;


        public CartWishListController(ICartWishListAPI cartwishlist, ICheckOutAPI checkout)
        {
            _cartwishlist = cartwishlist;
            _checkout=  checkout;
        }
        
        [HttpPost(nameof(AddWishList))]
        public async Task<IActionResult> AddWishList(WishList req)
        {
            var res = _cartwishlist.AddWishList(req, GetToken()).Result;
            return Json(res);
        }
        
        [HttpPost(nameof(MoveItemWishListToCart))]
        public async Task<IActionResult> MoveItemWishListToCart(int Id)
        {
            var res = _cartwishlist.MoveItemWishListToCart(Id, GetToken()).Result;
            return Json(res);
        }
        
        [HttpPost(nameof(AddToCart))]
        public async Task<IActionResult> AddToCart(CartItem req)
        {
            req.Qty = req.Qty==0?1:req.Qty;
            var res = _cartwishlist.AddToCart(req, GetToken()).Result;
            return Json(res);
        }
        
        [HttpPost(nameof(DeleteCart))]
        public async Task<IActionResult> DeleteCart(CartItem req)
        {
            var res = _cartwishlist.DeleteCart(req, GetToken()).Result;
            return Json(res);
        }
        
        [HttpPost(nameof(WishListSlide))]
        public async Task<IActionResult> WishListSlide()
        {
            var res = _cartwishlist.GetWishListSlide(GetToken()).Result;
            return View("Partial/_wishListSlide", res);
        }
        
        [HttpPost(nameof(CartSlide))]
        public async Task<IActionResult> CartSlide()
        {
            var res = _cartwishlist.GetCartListSlide(GetToken()).Result;
            return View("Partial/_cartSlide", res);
        }
        
        [HttpPost(nameof(CartWishListCount))]
        public async Task<IActionResult> CartWishListCount()
        {
            var res = _cartwishlist.GetCartwishListCount(GetToken()).Result;
            return Json(res);
        }
        
        [HttpGet(nameof(CartDetails))]
        public async Task<IActionResult> CartDetails()
        {
            return View();
        }
        
        [HttpPost(nameof(_CartDetails))]
        public async Task<IActionResult> _CartDetails()
        {
            var res = _cartwishlist.GetCartListSlide(GetToken()).Result;
            return PartialView("Partial/_cartDetails", res);
        }

        [HttpPost(nameof(_CartPlaceOrder))]
        public async Task<IActionResult> _CartPlaceOrder(bool IsBuyNow = false, int PaymentMode=1)
        {
            var res = await _cartwishlist.GetCartListSlide(GetToken(), IsBuyNow);
            var offerRes = await _checkout.GetAllCoupon(PaymentMode, GetToken());
            res.Result.Coupons = offerRes;
            return PartialView("~/Views/CheckOut/partial/_placeorder.cshtml", res);
        }
        
        [HttpPost(nameof(WishListToCart))]
        public async Task<IActionResult> WishListToCart(int id)
        {
            var res = _cartwishlist.MoveItemWishListToCart(id,GetToken()).Result;
            return Json(res);
        }

        [HttpPost(nameof(DeleteWishList))]
        public async Task<IActionResult> DeleteWishList(int id)
        {
            var res = _cartwishlist.DeleteWishListItem(id, GetToken()).Result;
            return Json(res);
        }
        
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }

        [HttpGet(nameof(Wishlist))]
        public async Task<IActionResult> Wishlist()
        {
            var res = _cartwishlist.GetWishListSlide(GetToken()).Result;
            return View(res);
        }
        
        [HttpPost(nameof(MoveAllToCart))]
        public async Task<IActionResult> MoveAllToCart()
        {
            var res = _cartwishlist.MoveAllItemWishListToCart(User.GetLoggedInUserToken()).Result;
            return Json(res);
        }
    }
}
