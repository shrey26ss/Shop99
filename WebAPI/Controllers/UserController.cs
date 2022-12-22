using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("/api/")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userservice;

        public UserController(IUserRepo userservice) => _userservice = userservice;
        [Route("User/CustomerList")]
        public async Task<IActionResult> CustomerList() => Ok(await _userservice.GetUserListByRole(Entities.Enums.Role.Customer));
        [Route("User/VendorList")]
        public async Task<IActionResult> VendorList() => Ok(await _userservice.GetUserListByRole(Entities.Enums.Role.Vendor));
    }
}
