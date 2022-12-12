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
   // [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/{controller}")]
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
            return Ok(await _cartwishlist.AddWishList(new RequestBase<WishList>
            {
                Data = req
            }));
        }
        [HttpPost(nameof(AddCartItem))]
        public async Task<IActionResult> AddCartItem(CartItem req)
        {
            return Ok(await _cartwishlist.AddToCart(new RequestBase<CartItem>
            {
                Data = req
            }));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(GetCartList))]
        public async Task<IActionResult> GetCartList()
        {
            return Ok(await _cartwishlist.GetCartList(new Request { LoginId = User.GetLoggedInUserId<int>() }));
        }

    }
}
