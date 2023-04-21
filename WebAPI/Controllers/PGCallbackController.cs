using AutoMapper;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateWay.PaymentGateway.PayU;
using Service.API;
using Service.CartWishList;
using Service.Identity;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("/api/{controller}")]
    public class PGCallbackController : ControllerBase
    {
        private readonly IPGCallback _ipgcallback;
        public PGCallbackController(IPGCallback ipgcallback)
        {
            _ipgcallback = ipgcallback;
        }
        [HttpPost(nameof(PayUnotify))]
        public async Task<IActionResult> PayUnotify(PayUResponse req)
        {
            return Ok(await _ipgcallback.PayUnotify(new RequestBase<PayUResponse>
            {
                Data = req, LoginId = User.GetLoggedInUserId<int>()
            }));
        }
    }
}
