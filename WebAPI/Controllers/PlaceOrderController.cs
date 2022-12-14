using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.API;
using Service.Identity;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/{controller}")]
    public class PlaceOrderController : ControllerBase
    {
        private readonly IPlaceOrder _placeorder;
        public PlaceOrderController(IPlaceOrder placeorder)
        {
            _placeorder = placeorder;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost(nameof(GetPaymentMode))]
        public async Task<IActionResult> GetPaymentMode()
        {
            return Ok(await _placeorder.GetPaymentMode());
        }
    }
}
