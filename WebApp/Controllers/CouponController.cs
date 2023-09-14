using AppUtility.APIRequest;
using AppUtility.Helper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
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
            List<Coupon> coupons = await GetCoupons(Id);
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
        public async Task<IActionResult> CreateCoupon(int Id =0)
        {
            var cpn = new Coupon();
            if(Id != 0)
            {
                string tokens = User.GetLoggedInUserToken();
                var couponres = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/GetCoupons", JsonConvert.SerializeObject(new SearchItem { Id = Id }), tokens);
                if(couponres.HttpStatusCode== HttpStatusCode.OK)
                {
                    var deserializeObj = JsonConvert.DeserializeObject<Response<List<Coupon>>>(couponres.Result);
                    cpn = deserializeObj.Result.FirstOrDefault();
                }
            }
            return PartialView(cpn);
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateAjax]
        public async Task<ActionResult> CreateCoupon(Coupon offers)
        {
            Response response = new Response();
            try
            {
                var couponRes = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Offers/AddUpdateCoupon", JsonConvert.SerializeObject(offers), User.GetLoggedInUserToken());
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
    }
}
