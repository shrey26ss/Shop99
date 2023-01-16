using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class Notify : ControllerBase
    {
        private readonly INotifyService _notify;
        public Notify(INotifyService notify)
        {
            _notify = notify;
        }
        [Route("Notify/SaveSMSEmailWhatsappNotification")]
        public async Task<IActionResult> SaveSMSEmailWhatsappNotification(SMSEmailWhatsappNotification req) => Ok(await _notify.SaveSMSEmailWhatsappNotification(req,User.GetLoggedInUserId<int>()));

        [AllowAnonymous]
        [Route("Notify/GetNotify")]
        [HttpGet]
        public async Task<ActionResult> GetNotify() => Ok(await _notify.GetNotify());

    }
}
