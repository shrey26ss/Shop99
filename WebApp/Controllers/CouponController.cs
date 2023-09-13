using AppUtility.APIRequest;
using AppUtility.Helper;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
    }
}
