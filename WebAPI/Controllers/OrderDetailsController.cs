﻿using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
  
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _orderRepo;

        public OrderDetailsController(IOrderDetailsService orderRepo) => _orderRepo = orderRepo;
        [HttpPost]
        [Route("OrderDetails/GetDetails")]
        public async Task<IActionResult> GetDetails(OrderDetailsRequest req)
        {
            return Ok(await _orderRepo.GetAsync(User.GetLoggedInUserId<int>(), req));
        }
        [HttpPost]
        [Route("OrderDetails/ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(OrderDetailsRow req) => Ok(await _orderRepo.ChengeStatusAsync(User.GetLoggedInUserId<int>(),req));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("OrderDetails/UpdateShippingNInvoice")]
        public async Task<IActionResult> UpdateShippingNInvoice(OrderShippedStatus req) => Ok(await _orderRepo.UpdateShippingNInvoice(req));
        [HttpPost]
        [Route("OrderDetails/GetInvoiceDetails")]
        public async Task<IActionResult> GetInvoiceDetails(OrderInvoiceRequest req) => Ok(await _orderRepo.GetInvoiceDetails(req.OrderId));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("OrderDetails/OrderReplacedConform")]
        public async Task<IActionResult> OrderReplacedConform(OrderReplacedConformReq req) => Ok(await _orderRepo.OrderReplacedConform(req));
       [HttpPost]
        [Route("OrderDetails/GetReturnRequest")]
        public async Task<IActionResult> GetReturnRequest(OrderDetailsRequest request) => Ok(await _orderRepo.GetReturnRequest(request));
        [HttpPost]
        [Route("OrderDetails/GetUsersOrderTraking")]
        public async Task<IActionResult> GetUsersOrderTraking(OrderReplacedConformReq req) => Ok(await _orderRepo.GetUsersOrderTraking(req));
        [HttpPost]
        [Route("OrderDetails/GetReturnRequestByOrderId")]
        public async Task<IActionResult> GetReturnRequestByOrderId(OrderDetailsRequest req) => Ok(await _orderRepo.GetReturnRequestByOrderId(req));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("OrderDetails/GetOrderGSTDetails")]
        public async Task<IActionResult> GetOrderGSTDetails(OrderDetailsRequest req) => Ok(await _orderRepo.GetOrderGST(User.GetLoggedInUserId<int>(),req));
    }
}
