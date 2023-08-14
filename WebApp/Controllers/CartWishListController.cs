using Entities.Models;
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
        public CartWishListController(ICartWishListAPI cartwishlist)
        {
            _cartwishlist = cartwishlist;
           
        }
        [Route("AddWishList")]
        [HttpPost]
        public async Task<IActionResult> AddWishList(WishList req)
        {
            var res = _cartwishlist.AddWishList(req, GetToken()).Result;
            return Json(res);
        }
        [Route("MoveItemWishListToCart")]
        [HttpPost]
        public async Task<IActionResult> MoveItemWishListToCart(int Id)
        {
            var res = _cartwishlist.MoveItemWishListToCart(Id, GetToken()).Result;
            return Json(res);
        }
        [Route("AddToCart")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItem req)
        {
            req.Qty = req.Qty==0?1:req.Qty;
            var res = _cartwishlist.AddToCart(req, GetToken()).Result;
            return Json(res);
        }
        [Route("DeleteCart")]
        [HttpPost]
        public async Task<IActionResult> DeleteCart(CartItem req)
        {
            var res = _cartwishlist.DeleteCart(req, GetToken()).Result;
            return Json(res);
        }
        [Route("WishListSlide")]
        [HttpPost]
        public async Task<IActionResult> WishListSlide()
        {
            var res = _cartwishlist.GetWishListSlide(GetToken()).Result;
            return View("Partial/_wishListSlide", res);
        }
        [Route("CartSlide")]    
        [HttpPost]
        public async Task<IActionResult> CartSlide()
        {
            var res = _cartwishlist.GetCartListSlide(GetToken()).Result;
            return View("Partial/_cartSlide", res);
        }
        [Route("CartWishListCount")]
        [HttpPost]
        public async Task<IActionResult> CartWishListCount()
        {
            var res = _cartwishlist.GetCartwishListCount(GetToken()).Result;
            return Json(res);
        }
        [Route("CartDetails")]
        [HttpGet]
        public async Task<IActionResult> CartDetails()
        {
            return View();
        }
        [Route("_CartDetails")]
        [HttpPost]
        public async Task<IActionResult> _CartDetails()
        {
            var res = _cartwishlist.GetCartListSlide(GetToken()).Result;
            return PartialView("Partial/_cartDetails", res);
        }
        [Route("_CartPlaceOrder")]
        [HttpPost]
        public async Task<IActionResult> _CartPlaceOrder(bool IsBuyNow = false)
        {
            var res = _cartwishlist.GetCartListSlide(GetToken(), IsBuyNow).Result;
            return PartialView("~/Views/CheckOut/partial/_placeorder.cshtml", res);
        }
        [Route("WishListToCart")]
        [HttpPost]
        public async Task<IActionResult> WishListToCart(int id)
        {
            var res = _cartwishlist.MoveItemWishListToCart(id,GetToken()).Result;
            return Json(res);
        }
        [Route("DeleteWishList")]
        [HttpPost]
        public async Task<IActionResult> DeleteWishList(int id)
        {
            var res = _cartwishlist.DeleteWishListItem(id, GetToken()).Result;
            return Json(res);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
        [Route("wishlist")]
        [HttpGet]
        public async Task<IActionResult> Wishlist()
        {
            var res = _cartwishlist.GetWishListSlide(GetToken()).Result;
            return View(res);
        }
        [Route("MoveAllToCart")]
        [HttpPost]
        public async Task<IActionResult> MoveAllToCart()
        {
            var res = _cartwishlist.MoveAllItemWishListToCart(User.GetLoggedInUserToken()).Result;
            return Json(res);
        }
    }
}
