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
    [Route("/api/CartWishList")]
    public class CartWishList : ControllerBase
    {
        private readonly ICartWishListService _cartwishlist;

        public CartWishList(ICartWishListService cartwishlist)
        {
            _cartwishlist = cartwishlist;
        }
     
    
        [HttpPost(nameof(AddWishList))]
        public async Task<IActionResult> AddWishList(WishList req)
        {
            req.EntryBy = User.GetLoggedInUserId<int>();
            return Ok(await _cartwishlist.AddWishList(new RequestBase<WishList>
            {
                Data = req
            }));
        }
        //[Authorize(AuthenticationSchemes = "Bearer")]
     
        [HttpPost(nameof(AddCartItem))]
        public async Task<IActionResult> AddCartItem(CartItem req)
        {
            req.UserID = User.GetLoggedInUserId<int>();
            return Ok(await _cartwishlist.AddToCart(new RequestBase<CartItem>
            {
                Data = req
            }));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpPost(nameof(DeleteCart))]
        public async Task<IActionResult> DeleteCart(CartItem req)
        {
            req.UserID = User.GetLoggedInUserId<int>();
            return Ok(await _cartwishlist.DeleteCart(new RequestBase<CartItem>
            {
                Data = req
            }));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(GetWishlist))]
        public async Task<IActionResult> GetWishlist()
        {
            return Ok(await _cartwishlist.GetWishlist(new Request { LoginId = User.GetLoggedInUserId<int>() }));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(GetCartItemlist))]
        public async Task<IActionResult> GetCartItemlist(bool IsBuyNow)
        {
            return Ok(await _cartwishlist.GetCartItemlist(new Request { LoginId = User.GetLoggedInUserId<int>() }, IsBuyNow));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(CartWishListCount))]
        public async Task<IActionResult> CartWishListCount() => Ok(await _cartwishlist.CartWishListCount(new Request { LoginId = User.GetLoggedInUserId<int>() }));
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(AddWishListToCart))]
        public async Task<IActionResult> AddWishListToCart(SearchItem req) => Ok(await _cartwishlist.AddWishListToCart(req));
        [HttpPost(nameof(DeleteWishListItem))]
        public async Task<IActionResult> DeleteWishListItem(SearchItem req) => Ok(await _cartwishlist.DeleteWishListItem(req));
        [HttpPost(nameof(MoveAllItemWishListToCart))]
        public async Task<IActionResult> MoveAllItemWishListToCart() => Ok(await _cartwishlist.MoveAllItemWishListToCart(User.GetLoggedInUserId<int>()));

    }
}
