using AppUtility.APIRequest;
using AppUtility.Helper;
using AutoMapper;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode.Attributes;
using WebApp.AppCode.Helper;
using WebApp.Middleware;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private string _apiBaseURL;
        private readonly IGenericMethods _convert;
        public OrderDetailsController(ILogger<OrderDetailsController> logger, IMapper mapper, AppSettings appSettings, IGenericMethods convert)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _convert = convert;
        }
        public IActionResult Index(StatusType type)
        {
            return View(type);
        }
        public async Task<IActionResult> OrderList(OrderDetailsRequest request)
        {
            return PartialView("PartialView/_OrderList", await GetList(request).ConfigureAwait(false));
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
            var res = await GetList(new OrderDetailsRequest()).ConfigureAwait(false);
            return PartialView("PartialView/_UserOrderHistory", res);
        }
        public async Task<IActionResult> MarkAsShippV(int Id)
        {
            return PartialView("PartialView/_MarkAsShippV", new OrderShippedStatus { Id = Id});
        }
        public IActionResult ShareTrackingDetails(TrackingModel model)
        {
            return PartialView("PartialView/_ShareTrackingDetails", model);
        }
        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> UpdateShippingNInvoice(OrderShippedStatus model)
        {
            Response response = new Response();
            try
            {
                var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/UpdateShippingNInvoice", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
                if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
                    response = deserializeObject;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
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
        public async Task<IActionResult> Invoice(int OrderId = 0)
        {
            var res = await _convert.GetItem<OrderInvoice>("OrderDetails/GetInvoiceDetails", User.GetLoggedInUserToken(), new OrderInvoiceRequest { OrderId=OrderId});
            return View(res);
        }

        #region Private Mathod
        private async Task<List<OrderDetailsColumn>> GetList(OrderDetailsRequest request)
        {
            List<OrderDetailsColumn> list = new List<OrderDetailsColumn>();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/GetDetails", JsonConvert.SerializeObject(request), User.GetLoggedInUserToken());
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
