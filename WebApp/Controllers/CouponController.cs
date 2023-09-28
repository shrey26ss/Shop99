using AppUtility.APIRequest;
using AppUtility.Helper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode.Attributes;
using WebApp.Middleware;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CouponController : Controller
    {
        private string _apiBaseURL;
        private IDDLHelper _ddl;

        public CouponController(AppSettings appSettings, IDDLHelper ddl) //IRepository<EmailConfig> emailConfig, 
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _ddl = ddl;
        }

        [HttpGet("/Coupons")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Coupon/CouponsList")]
        public async Task<ActionResult> CouponsList(int Id)
        {
            List<Coupon>coupons = await GetCoupons(Id);
            return PartialView(coupons);
        }
        private async Task<List<Coupon>> GetCoupons(int Id)
        {
            List<Coupon> coupon = new List<Coupon>();
            string _token = User.GetLoggedInUserToken();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/GetCoupons", JsonConvert.SerializeObject(new SearchItem { Id = Id }), _token);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<Coupon>>>(apiResponse.Result);
                coupon = deserializeObject.Result;
            }
            return coupon;
        }
        [Authorize(Roles ="1")]

        public async Task<IActionResult> CreateCoupon(int CouponId = 0)
        {
            var cpn = new Coupon();
            if(CouponId != 0)
            {
                string tokens = User.GetLoggedInUserToken();
                var couponres = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/GetCoupons", JsonConvert.SerializeObject(new SearchItem {Id = CouponId }), tokens);
                if(couponres.HttpStatusCode== HttpStatusCode.OK)
                {
                    var deserializeObj = JsonConvert.DeserializeObject<Response<List<Coupon>>>(couponres.Result);
                    cpn = deserializeObj.Result.FirstOrDefault();
                }
            }
            return PartialView(cpn);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAjax]
        public async Task<ActionResult> CreateCoupon(Coupon cop)
        {
            Response response = new Response();
            try
            {
                var couponRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/AddUpdateCoupon", JsonConvert.SerializeObject(cop), User.GetLoggedInUserToken());
                if (couponRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    response = JsonConvert.DeserializeObject<Response>(couponRes.Result);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> DelCoupon(int CouponId)
        {
            Response response = new Response();
            try
            {
                var copn = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/DelCoupon?couponId={CouponId}", null, User.GetLoggedInUserToken());
                if (copn.HttpStatusCode == HttpStatusCode.OK)
                {
                    response = JsonConvert.DeserializeObject<Response>(copn.Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(response);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> UpdateIsActive(CouponUpdateIsActive req)
        {
            var response = new Response();
            if (req.CouponId >= 1)
            {
                string _token = User.GetLoggedInUserToken();
                var categoryrRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/UpdateIsActiveCoupon", JsonConvert.SerializeObject(req), _token);
                if (categoryrRes.HttpStatusCode == HttpStatusCode.OK)
                {
                    response = JsonConvert.DeserializeObject<Response>(categoryrRes.Result);
                }
            }
            return Json(response);
        }

    }
}
