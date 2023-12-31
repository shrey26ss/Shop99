﻿using AppUtility.APIRequest;
using AppUtility.Helper;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.AppCode;
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
        private readonly string _apiBaseURL;
        private readonly IGenericMethods _convert;
        private readonly ILogger _logger;
        private readonly IHttpRequestInfo _httpInfo;
        public OrderDetailsController(ILogger<OrderDetailsController> logger, AppSettings appSettings, IGenericMethods convert, IHttpRequestInfo httpInfo)
        {
            _apiBaseURL = appSettings.WebAPIBaseUrl;
            _convert = convert;
            _logger = logger;
            _httpInfo = httpInfo;
        }
        public IActionResult Index(StatusType type)
        {
            return View(type);
        }
        public async Task<IActionResult> OrderList(OrderDetailsRequest request)
        {
            var res = await GetList(request).ConfigureAwait(false);
            ViewBag.loginid = User.GetLoggedInUserId<int>();
            return PartialView("PartialView/_OrderList", res);
        }
        public async Task<IActionResult> OrderReportExcel(int Top = 50, string FromDate = "", string ToDate = "", StatusType StatusID = 0, string SearchText = "")
        {
            var res = await GetList(new OrderDetailsRequest
            {
                Top = Top,
                FromDate = FromDate,
                ToDate = ToDate,
                StatusID = StatusID,
                SearchText = SearchText
            }).ConfigureAwait(false);
            DataTable dataTable = ConvertToDataTable.ToDataTable(res);
            string[] removbleCol = { "ID", "VarriantID", "OrderID", "EntryBy", "Description", "SKU", "Discount", "OtherCharge", "Total", "UserID", "Thumbnail", "PaymentModeId", "Remark", "DocketNo", "ReturnTillOn" };
            var exportToExcel = Utility.O.ExportToExcel(dataTable, removbleCol);
            return File(exportToExcel.data, DOCType.XlsxContentType, "Order.xlsx");
        }
        public async Task<IActionResult> OrderGSTExcel(int Top = 50, string FromDate = "", string ToDate = "", StatusType StatusID = 0, string SearchText = "")
        {
            var req = new OrderDetailsRequest
            {
                Top = Top,
                FromDate = FromDate,
                ToDate = ToDate,
                StatusID = StatusID,
                SearchText = SearchText
            };
            var res = await _convert.GetList<OrderGSTDetails>("OrderDetails/GetOrderGSTDetails", User.GetLoggedInUserToken(), req);
            DataTable dataTable = ConvertToDataTable.ToDataTable(res.ToList());
            string[] removbleCol = { "" };
            var exportToExcel = Utility.O.ExportToExcel(dataTable, removbleCol);
            return File(exportToExcel.data, DOCType.XlsxContentType, "OrderGSTReport.xlsx");
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
            var res = await _convert.GetItem<OrderInvoice>("OrderDetails/GetInvoiceDetails", User.GetLoggedInUserToken(), new OrderInvoiceRequest { OrderId = Id });
            return PartialView("PartialView/_MarkAsShippV", new OrderShippedStatus { Id = Id, InvoiceNumber = res.InvoiceNo });
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
                _logger.LogError(ex.Message, ex);
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(OrderDetailsVM model)
        {
            var res = new Response();
            if (model.StatusID == StatusType.Confirmed)
                model.InvoiceNumber = Helper.O.GenerateInvoiceNumber(model.ID);
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/ChangeStatus", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                res = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(res);
        }
        public async Task<IActionResult> Invoice(int OrderId = 0)
        {

            var res = await _convert.GetItem<OrderInvoice>("OrderDetails/GetInvoiceDetails", User.GetLoggedInUserToken(), new OrderInvoiceRequest { OrderId = OrderId });

            Guid orderId;
            var res1 = await GetList(new OrderDetailsRequest()).ConfigureAwait(false);
            foreach (var item in res1)
            {
                if (OrderId == item.ID)
                {
                    res.OrderID = item.OrderID;
                    break;
                }
            }
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> OrderReplacedConform(OrderReplacedConformReq model)
        {
            var res = new Response();
            model.Role = User.GetLoggedInUserRoles();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/OrderReplacedConform", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                res = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(res);
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
        [HttpGet]
        public IActionResult ReturnRequest()
        {
            return View();
        }
        public async Task<IActionResult> ReturnRequestList(OrderDetailsRequest req)
        {
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/GetReturnRequest", JsonConvert.SerializeObject(req), User.GetLoggedInUserToken());
            var response = JsonConvert.DeserializeObject<Response<List<ReturnRequestList>>>(apiResponse.Result);
            return PartialView("PartialView/_ReturnRequestList", response.Result);
        }
        public async Task<IActionResult> ReturnOrderReportExcel(StatusType StatusID = 0)
        {
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/GetReturnRequest", JsonConvert.SerializeObject(new OrderDetailsRequest { StatusID = StatusID }), User.GetLoggedInUserToken());
            var response = JsonConvert.DeserializeObject<Response<List<ReturnRequestList>>>(apiResponse.Result);
            var res = response.Result;
            DataTable dataTable = ConvertToDataTable.ToDataTable(res);
            string[] removbleCol = { "ID", "Thumbnail" };
            var exportToExcel = Utility.O.ExportToExcel(dataTable, removbleCol);
            return File(exportToExcel.data, DOCType.XlsxContentType, "ReturnOrder.xlsx");
        }
        [Route("UserOrderDetails")]
        public async Task<IActionResult> UsersOrderTraking(int OrderId)
        {
            OrderReplacedConformReq model = new OrderReplacedConformReq();
            model.ID = OrderId;
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/GetUsersOrderTraking", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
          
            var response = JsonConvert.DeserializeObject<UsersOrderTrakingViewModel>(apiResponse.Result);
            
            return View(response);
        }

        [HttpPost]
        public IActionResult ReturnOrder(int ID = 1)
        {
            OrderDetailsVM model = new OrderDetailsVM();

            model.ID = ID;
            return PartialView("PartialView/_ReturnOrder", model);
        }

        [HttpPost("PlaceReturnOrder")]
        public async Task<IActionResult> PlaceReturnOrder(OrderDetailsVM model)
        {
            var res = new Response();
            if (String.IsNullOrEmpty(model.Remark))
                return Json(new Response { ResponseText = "Please Enter Reason", StatusCode = ResponseStatus.Failed });
            model.ImagePaths = UploadImage(model.Files, model.ID);
            model.StatusID = StatusType.ReturnInitiated;
            if (model.StatusID == StatusType.Confirmed)
                model.InvoiceNumber = Helper.O.GenerateInvoiceNumber(model.ID);
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OrderDetails/ChangeStatus", JsonConvert.SerializeObject(model), User.GetLoggedInUserToken());
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                res = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            return Json(res);
        }
        private string UploadImage(List<IFormFile> req, int OrderId)
        {
            var ImagePath = new List<string>();
            if (req != null)
            {
                int counter = 0;
                foreach (var item in req)
                {
                    counter++;
                    string fileName = $"{counter.ToString() + DateTime.Now.ToString("ddMMyyyyhhmmssmmm") + OrderId.ToString()}.jpeg";
                    string filePath = FileDirectories.ReplaceOrderImage.Replace("//", "/");
                    Utility.O.UploadFile(new FileUploadModel
                    {
                        file = item,
                        FileName = fileName,
                        FilePath = filePath,
                        IsThumbnailRequired = false,
                    });
                    ImagePath.Add(string.Concat(_httpInfo.AbsoluteURL() + "/", FileDirectories.ReplaceOrderImageSuffixDefault, fileName));
                }
            }
            return string.Join(',', ImagePath);
        }
        public async Task<IActionResult> GetReturnRequestByOrderId(int Id)
        {
            var res = await _convert.GetItem<ReturnRequestList>("OrderDetails/GetReturnRequestByOrderId", User.GetLoggedInUserToken(), new { Id });
            return PartialView("PartialView/_GetReturnRequestByOrderId", res ?? new ReturnRequestList());
        }
    }
}
