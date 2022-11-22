using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Identity;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class VariantController : Controller
    {
        private readonly IVariant _variant;
        private readonly UserManager<ApplicationUser> _userManager;
        public VariantController(IVariant variant, UserManager<ApplicationUser> userManager)
        {
            _variant = variant;
            _userManager = userManager;
        }
        [Route("Variant/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateVariant(Variants req)
        {
            return Ok(await _variant.AddUpdate(new RequestBase<Variants>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

        [Route("Variant/GetVariants")]
        public async Task<IActionResult> GetVariants(SearchItem req)
        {
            return Ok(await _variant.GetVariants(new RequestBase<SearchItem>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }

    }
}
