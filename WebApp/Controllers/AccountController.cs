using AutoMapper;
using Entities.Enums;
using Hangfire;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Identity;
using Services.Identity;
using System.Data;
using System.Threading.Tasks;
using System;
using WebApp.Models.ViewModels;
using System.Linq;
using Entities.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using WebApp.Models;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Service.API;
using AppUtility.APIRequest;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using WebApp.Middleware;
using Service.Models;
using WebApp.AppCode.Attributes;
using Newtonsoft.Json.Linq;
using AppUtility.Helper;

namespace WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        #region Variables
        //private readonly JWTConfig _jwtConfig;
        //private readonly ApplicationUserManager _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private IUserService _users;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //private readonly IRepository<EmailConfig> _emailConfig;
        //private readonly ITokenService _tokenService;
        private IMapper _mapper;
        private string _apiBaseURL;
        #endregion

        public AccountController(ILogger<AccountController> logger, IMapper mapper, AppSettings appSettings, SignInManager<ApplicationUser> signInManager) //IRepository<EmailConfig> emailConfig, _emailConfig = emailConfig;
        {
            _logger = logger;
            _mapper = mapper;
            //_userManager = userManager;
            //_roleManager = roleManager;
            _signInManager = signInManager;
            //_users = users;
            //_tokenService = tokenService;
            _apiBaseURL = appSettings.WebAPIBaseUrl;
        }

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAjax]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var response = new Response();
            var apiResponse = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/register", JsonConvert.SerializeObject(model), null);
            if (apiResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<Response>(apiResponse.Result);
            }
            //if (response.StatusCode == ResponseStatus.Success)
            //{
            //    return PartialView();
            //}
            return Json(response);
        }
        #endregion

        #region Login
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel { IsTwoFactorEnabled = false });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            try
            {
                var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/Login", JsonConvert.SerializeObject(model));
                if (Response.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deserializeObject = JsonConvert.DeserializeObject<Response<AuthenticateResponse>>(Response.Result);

                    if (deserializeObject.StatusCode == ResponseStatus.Success)
                    {
                        if (deserializeObject.StatusCode == ResponseStatus.Success)
                        {
                            var applicationUser = deserializeObject.Result;
                            ApplicationUser user = new ApplicationUser
                            {
                                Id = applicationUser.Id,
                                Name = applicationUser.Name,
                                RefreshToken = applicationUser.RefreshToken,
                                Role = applicationUser.Role,
                                Token = applicationUser.Token
                            };
                            if (applicationUser.Role.Equals("3"))// Vendor
                            {
                                user.Role = "0";
                                ReturnUrl = ReturnUrl?.Trim() == "/" ? "/Vendor/Index" : ReturnUrl;
                            }
                            else if (applicationUser.Role.Equals("1")) // Admin
                            {
                                ReturnUrl = ReturnUrl?.Trim() == "/" ? "/dashboard" : ReturnUrl;
                            }
                            else if (applicationUser.Role.Equals("2")) // Customer
                            {
                                ReturnUrl = ReturnUrl?.Trim() == "/" ? "/" : ReturnUrl;
                            }
                            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                            identity.AddClaim(new Claim("Id", user.Id.ToString()));
                            identity.AddClaim(new Claim("Token", user.Token.ToString()));
                            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name ?? string.Empty));
                            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role ?? string.Empty));
                            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
                            return LocalRedirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                    }
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, Response.HttpMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), new { this.GetType().Name, fn = nameof(this.Login) });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LoginWithOTP(LoginViewModel model, string ReturnUrl)
        {
            var res = new LoginViewModel();
            if (!string.IsNullOrEmpty(model.MobileNo))
            {
                if (model.MobileNo.Length < 10)
                {
                    ModelState.AddModelError(string.Empty, "Please Enter Valid Mobile Number");
                    return View("Login", res);
                }
            }            
            if (string.IsNullOrEmpty(model.OTP))
            {
                var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/SendLoginOTP", JsonConvert.SerializeObject(model));
                var deserializeObject = JsonConvert.DeserializeObject<Response>(Response.Result);
                if (deserializeObject.StatusCode == ResponseStatus.Failed)
                {
                    ModelState.AddModelError(string.Empty, deserializeObject.ResponseText);
                }
                if (deserializeObject.StatusCode == ResponseStatus.Success)
                {
                    res.StatusCode = "Success";
                }
                return View("Login", res);
            }
            else
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/");
                try
                {
                    var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/LoginWithOTP", JsonConvert.SerializeObject(model));
                    if (Response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<Response<AuthenticateResponse>>(Response.Result);

                        if (deserializeObject.StatusCode == ResponseStatus.Success)
                        {
                            if (deserializeObject.StatusCode == ResponseStatus.Success)
                            {
                                var applicationUser = deserializeObject.Result;
                                ApplicationUser user = new ApplicationUser
                                {
                                    Id = applicationUser.Id,
                                    Name = applicationUser.Name,
                                    RefreshToken = applicationUser.RefreshToken,
                                    Role = applicationUser.Role,
                                    Token = applicationUser.Token
                                };
                                if (applicationUser.Role.Equals("3"))// Vendor
                                {
                                    user.Role = "0";
                                    ReturnUrl = ReturnUrl?.Trim() == "/" ? "/Vendor/Index" : ReturnUrl;
                                }
                                else if (applicationUser.Role.Equals("1")) // Admin
                                {
                                    ReturnUrl = ReturnUrl?.Trim() == "/" ? "/dashboard" : ReturnUrl;
                                }
                                else if (applicationUser.Role.Equals("2")) // Customer
                                {
                                    ReturnUrl = ReturnUrl?.Trim() == "/" ? "/" : ReturnUrl;
                                }
                                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                                identity.AddClaim(new Claim("Id", user.Id.ToString()));
                                identity.AddClaim(new Claim("Token", user.Token.ToString()));
                                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name ?? string.Empty));
                                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role ?? string.Empty));
                                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
                                if (!string.IsNullOrEmpty(ReturnUrl))
                                {
                                    return LocalRedirect(ReturnUrl);
                                }
                                else
                                {
                                    res.StatusCode = "Success";
                                    return View("Login", res);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, deserializeObject.ResponseText);
                            res.StatusCode = "Success";
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, Response.HttpMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message.ToString(), new { this.GetType().Name, fn = nameof(this.Login) });
                }
            }
            return View("Login", res);
        }


        [HttpPost]
        public async Task<IActionResult> OtpRegistration(string  MobileNo)
        {
            var otpmodel = new LoginViewModel()
            {
                MobileNo= MobileNo
            };
            var Response = await AppWebRequest.O.PostAsync($"{_apiBaseURL}/api/OtpRegistration", JsonConvert.SerializeObject(otpmodel));
            return Json(Response.Result);
        }
        #endregion



        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return LocalRedirect(returnUrl);
        }
    }
}
