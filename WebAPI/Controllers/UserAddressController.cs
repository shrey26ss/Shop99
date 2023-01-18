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
   
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class UserAddressController : Controller
    {
        public readonly IUserAddress _address;
        public UserAddressController(IUserAddress address) => _address = address;

        [HttpPost]
        [Route("UserAddress/AddUpdate")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdate(UserAddress req) => Ok(await _address.AddUpdate(new RequestBase<UserAddress>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));
        [HttpPost]
        [Route("UserAddress/GetAddress")]
        public async Task<IActionResult> GetAddress() => Ok(await _address.GetList(new Request { LoginId = User.GetLoggedInUserId<int>() }));
    }
}
