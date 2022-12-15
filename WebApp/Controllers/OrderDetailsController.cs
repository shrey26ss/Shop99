using AppUtility.APIRequest;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private string _apiBaseURL;
        public OrderDetailsController(ILogger<OrderDetailsController> logger, IMapper mapper, AppSettings appSettings)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> OrderList()
        {
            return PartialView("PartialView/_OrderList", await GetList().ConfigureAwait(false));
        }
        [Route("UserOrderHistory")]
        [HttpGet]
        public IActionResult UserOrderHistory()
        {
            return View();

        }
        [Route("_UserOrderHistory")]
        [HttpPost]
        public async Task<IActionResult> _UserOrderHistory()
        {
            var res = await GetList().ConfigureAwait(false);
            return PartialView("PartialView/_UserOrderHistory", res);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(OrderDetailsVM model)
        {
            var res = new Response();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/ChangeStatus", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                res = JsonConvert.DeserializeObject<Response> (apiResponse.Result);
            }
            return Json(res);
        }

        #region Private Mathod
        private async Task<List<OrderDetailsColumn>> GetList(int Id = 0)
        {
            List<OrderDetailsColumn> list = new List<OrderDetailsColumn>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/GetDetails", JsonConvert.SerializeObject(new SearchItem { Id = Id }), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var deserializeObject = JsonConvert.DeserializeObject<Response<List<OrderDetailsColumn>>>(apiResponse.Result);
                list = deserializeObject.Result;
            }
            return list;
        }

        #endregion
      
       
    }
}
