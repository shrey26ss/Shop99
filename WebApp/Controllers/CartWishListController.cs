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
            req.EntryBy = 10;
            var res = _cartwishlist.AddWishList(req).Result;
            return Json(res);
        }

        private string GetToken()
        {
            return User.GetLoggedInUserToken();
        }
    }
}
