using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("User/CustomerList")]
        public async Task<IActionResult> CustomerList() => Ok(await _userservice.GetUserListByRole(Role.Customer));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("User/VendorList")]
        public async Task<IActionResult> VendorList(VendorProfileRequest req = null) => Ok(await _userservice.GetVendorList(req));
      [HttpGet]
        [Route("User/GetUserById")]
        public async Task<IActionResult> GetUserById() => Ok(await _userservice.GetUserById(User.GetLoggedInUserId<int>()));
        [HttpPost]
        [Route("User/SaveProfileInfo")]
        public async Task<IActionResult> SaveProfileInfo(UserDetails req) => Ok(await _userservice.SaveProfileInfo(new RequestBase<UserDetails>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>()
        }));
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("User/ApproveVendorProfile")]
        public async Task<IActionResult> ApproveVendorProfile(VendorProfileRequest req) => Ok(await _userservice.ApproveVendorProfile(new RequestBase<VendorProfileRequest>
        {
            Data = req,
            LoginId = User.GetLoggedInUserId<int>(),
            RoleId = Convert.ToInt32(User.GetLoggedInUserRoles())
        }));

    }
}
