using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.API;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;
using System.Collections.Generic;
using System.Linq;
using WebApp.Middleware;
using System;
using Entities.Enums;
using AppUtility.Helper;
using WebApp.Models.ViewModels;
using Service.Models;
using WebApp.AppCode.Attributes;
using WebApp.Servcie;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartWishListController : Controller
    {
        private readonly ILogger<UserHomeController> _logger;
        private string _apiBaseURL;
        private readonly ICartWishListAPI _cartwishlist;
       

        public CartWishListController(ILogger<UserHomeController> logger, ICartWishListAPI cartwishlist)
        {
            _logger = logger;
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
        [Route("WishListToCart")]
        [HttpPost]
        public async Task<IActionResult> WishListToCart(int id)
        {
            var res = _cartwishlist.MoveItemWishListToCart(id,GetToken()).Result;
            return Json(res);
        }
        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }

    }
}
