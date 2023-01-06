using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class VendorController : ControllerBase
    {
        private readonly IVendor _vendor;

        public VendorController(IVendor vendor)
        {
            _vendor = vendor;
        }
        [Route("Vendor/ValidateVendor")]
        public async Task<IActionResult> ValidateVendor()
        {
            return Ok(await _vendor.ValidateVendor(new RequestBase<SearchItem>
            {
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Vendor/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateVendorDetails(VendorProfile req)
        {
            return Ok(await _vendor.AddUpdate(new RequestBase<VendorProfile>
            {
                Data = req,
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
        [Route("Vendor/VendorDetail")]
        public async Task<IActionResult> GetVendorDetails()
        {
            return Ok(await _vendor.GetVendorDetails(new RequestBase<SearchItem>
            {
                LoginId = User.GetLoggedInUserId<int>()
            }));
        }
    }
}
