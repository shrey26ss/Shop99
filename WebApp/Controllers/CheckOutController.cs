﻿using AppUtility.APIRequest;
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
    public class CheckOutController : Controller
    {
        private readonly ILogger<UserHomeController> _logger;
        private string _apiBaseURL;
        private readonly ICheckOutAPI _checkout;
       

        public CheckOutController(ILogger<UserHomeController> logger, ICheckOutAPI checkout)
        {
            _logger = logger;
            _checkout = checkout;
           
        }
        [Route("CheckOut")]
        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            return View();
        }
        [Route("GetUserAddress")]
        [HttpPost]
        public async Task<IActionResult> GetUserAddress()
        {
            var res = _checkout.GetUserGetAddress(GetToken()).Result;
            return PartialView("Partial/_address", res);
        }
        [Route("GetPaymentMode")]
        [HttpPost]
        public async Task<IActionResult> GetPaymentMode()
        {
            var res = _checkout.GetPaymentMode(GetToken()).Result;
            return Json(res);
        }
        [Route("PlaceOrder")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderReq req)
        {
            var res = _checkout.PlaceOrder(req, GetToken()).Result;
            return Json(req);
        }
        [Route("SaveAddress")]
        [HttpPost]
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
