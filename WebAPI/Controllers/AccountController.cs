﻿using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Service.Models;
using WebAPI.Middleware;
using System.Linq;
using System.Text;
using AppUtility.APIRequest;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Variables
        private readonly ApplicationUserManager _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenService _tokenService;
        private readonly INotifyService _notify;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;

        #endregion

        public AccountController(ApplicationUserManager userManager, ITokenService tokenService, ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor, INotifyService notify)
        {
            _logger = logger;
            _userManager = userManager;
            _notify = notify;
            _tokenService = tokenService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = _httpContextAccessor.HttpContext.RequestServices.GetService<SignInManager<ApplicationUser>>();
        }
        [AllowAnonymous]
        [HttpPost("/api/Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid Credentials"
            };
            try
            {

                var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginTwoStep), new
                    {
                        model.MobileNo,
                        model.RememberMe
                    });
                }
                else if (result.Succeeded)
                {
                    res = await GenerateAccessToken(model.MobileNo);
                }

            }
            catch (Exception ex)
            {
                res.ResponseText = "Something went wrong.Please tru after some time.";
                _logger.LogError(ex, ex.Message);
            }
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("/api/SendLoginOTP")]
        public async Task<IActionResult> SendLoginOTP(LoginViewModel model)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "It seems you are not registered with us. Please sign up.!"
            };
            try
            {
                var userExists = await _userManager.FindByMobileNoAsync(model.MobileNo);
                if (userExists?.Id < 1)
                    return Ok(res);
                model.Password = AppUtility.Helper.Utility.O.GenrateRandom(6, true);

                /* Send SMS here */
                #region send Notification
                await _notify.SaveSMSEmailWhatsappNotification(new SMSEmailWhatsappNotification() { FormatID = MessageFormat.OTP, IsSms = true, IsWhatsapp = true, PhoneNumber = model.MobileNo, OTP = model.Password }, User.GetLoggedInUserId<int>());
                #endregion
                /* End SMS */

                var result = await _userManager.SaveLoginOTP(model.MobileNo, model.Password);
                res.StatusCode = result.StatusCode;
                res.ResponseText = result.ResponseText;
            }
            catch (Exception ex)
            {
                res.ResponseText = "Something went wrong.Please tru after some time.";
                _logger.LogError(ex, ex.Message);
            }
            return Ok(res);
        }


        [AllowAnonymous]
        [HttpPost("/api/OtpRegistration")]
        public async Task<IActionResult> OtpRegistration(LoginViewModel model)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Mobile no Allready Exists Try another Mobile no."
            };
            try
            {
                model.Password = AppUtility.Helper.Utility.O.GenrateRandom(6, true);
                var userExists = await _userManager.FindByMobileNoAsync(model.MobileNo);
                if (userExists?.Id < 1)
                {
                    var result = await _userManager.SaveLoginOTP(model.MobileNo, model.Password);
                    if (result.StatusCode == ResponseStatus.Success)
                    {
                        await _notify.SaveSMSEmailWhatsappNotification(new SMSEmailWhatsappNotification() { FormatID = MessageFormat.OTP, IsSms = true, IsWhatsapp = true, PhoneNumber = model.MobileNo, OTP = model.Password }, User.GetLoggedInUserId<int>());
                        res.StatusCode = ResponseStatus.Success;
                        res.ResponseText = "Otp Sent On Your MobileNo.";
                    }
                    else
                    {
                        res.StatusCode = result.StatusCode;
                        res.ResponseText = result.ResponseText;
                    }
                }
                else if (!userExists.PhoneNumberConfirmed)
                {
                    var result = await _userManager.SaveLoginOTP(model.MobileNo, model.Password);
                    if (result.StatusCode == ResponseStatus.Success)
                    {
                        await _notify.SaveSMSEmailWhatsappNotification(new SMSEmailWhatsappNotification() { FormatID = MessageFormat.OTP, IsSms = true, IsWhatsapp = true, PhoneNumber = model.MobileNo, OTP = model.Password }, User.GetLoggedInUserId<int>());
                        res.StatusCode = ResponseStatus.Pending;
                        res.ResponseText = "Otp Sent On Your MobileNo.";
                    }
                    else
                    {
                        res.StatusCode = result.StatusCode;
                        res.ResponseText = result.ResponseText;
                    }
                }
            }
            catch (Exception ex)
            {
                res.ResponseText = "Something went wrong.Please tru after some time.";
                _logger.LogError(ex, ex.Message);
            }
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("/api/LoginWithOTP")]
        public async Task<IActionResult> LoginWithOTP(LoginViewModel model)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid OTP"
            };
            try
            {
                var result = await _userManager.SigninWithOTP(model.MobileNo, model.OTP, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    res = await GenerateAccessToken(model.MobileNo);
                }
            }
            catch (Exception ex)
            {
                res.ResponseText = "Something went wrong.Please tru after some time.";
                _logger.LogError(ex, ex.Message);
            }
            return Ok(res);
        }

        private async Task<Response<AuthenticateResponse>> GenerateAccessToken(string mobileNo)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid Credentials"
            };
            var user = await _userManager.FindByMobileNoAsync(mobileNo);
            if (user.Id > 0)
            {
                string IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string UserAgent = UserAgentAPI.FullInfo;
                string mergeToken = EncodeString(IP + UserAgent);
                var claims = new[] {
                        new Claim(ClaimTypesExtension.Id, user.Id.ToString()),
                        new Claim(ClaimTypesExtension.Role, user.Role??"CUSTOMER"),
                        new Claim(ClaimTypesExtension.UserName, user.UserName),
                        new Claim(ClaimTypesExtension.Name, user.Name),
                        new Claim(ClaimTypesExtension.SameSession, mergeToken)
                    };
                var token = _tokenService.GenerateAccessToken(claims);
                var authResponse = new AuthenticateResponse(user, token);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "Login Succussful";
                res.Result = authResponse;
            }
            return res;
        }
        private static string EncodeString(string serverName)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        }

        [HttpGet("/api/LoginTwoStep")]
        public async Task<IActionResult> LoginTwoStep(string email, bool rememberMe, string returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid attempt");
            }
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return BadRequest("Invalid attempt");
            }
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            //var message = new Message(new string[] { email }, "Authentication token", token, null);

            //await _emailSender.SendEmailAsync(message);

            return Ok(new { StatusCode = -3, ResponseText = "Two factor verification needed" });
        }

        [HttpPost("/api/LoginTwoStepPost")]
        public async Task<IActionResult> LoginTwoStep(TwoStepModel twoStepModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(twoStepModel);
            }
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                //return RedirectToAction(nameof(Error));
                return BadRequest("Error");
            }
            var result = await _signInManager.TwoFactorSignInAsync("Email", twoStepModel.TwoFactorCode, twoStepModel.RememberMe, rememberClient: false);
            if (result.Succeeded)
            {
                //return RedirectToLocal(returnUrl);
                return Ok("Success");
            }
            else if (result.IsLockedOut)
            {
                //Same logic as in the Login action
                ModelState.AddModelError("", "The account is locked out");
                return BadRequest("The account is locked out");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return BadRequest("Invalid Login Attempt");
            }
        }

        [HttpPost("/token/refresh")]
        public async Task<IActionResult> RefreshAsync(TokenApiModel tokenApiModel)
        {
            var response = new Response<AuthenticatedResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid client request",
            };
            if (tokenApiModel is null)
                return Ok(response); //return BadRequest("Invalid client request");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var id = principal.FindFirstValue("Id");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                response.StatusCode = ResponseStatus.Expired;
                response.ResponseText = ResponseStatus.Expired.ToString();
                return Ok(response);//return BadRequest("Invalid client request");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.RefreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(new Response<AuthenticatedResponse>
            {
                StatusCode = ResponseStatus.Success,
                ResponseText = ResponseStatus.Success.ToString(),
                Result = new AuthenticatedResponse()
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken.RefreshToken
                }
            });
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/api/register")]
        public async Task<IActionResult> Register(RegisterViewModels model)
        {
            var userExists = await _userManager.FindByMobileNoAsync(model.PhoneNumber);
            if (userExists?.Id > 0 && userExists.ToString() != "{}" && userExists.PhoneNumberConfirmed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { StatusCode = ResponseStatus.Failed, ResponseText = "User already exists!" });
            }
            else if (userExists?.Id > 0 && !userExists.PhoneNumberConfirmed)
            {
                var otpverify = await _userManager.ConfirmPhoneNumber(model.PhoneNumber, model.OTP, lockoutOnFailure: true);
                if (otpverify.Succeeded)
                {
                    return Ok(new Response { StatusCode = ResponseStatus.Success, ResponseText = "OTP Verified Successfull" });
                }
            }
            ApplicationUser user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                UserName = model.PhoneNumber.Trim(),
                Email = model.Email ?? "youremail@shop99.com",
                Role = model.Role.ToString(),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber.Trim(),
                RefreshToken = Guid.NewGuid().ToString().Replace("-", ""),
                RefreshTokenExpiryTime = DateTime.Now.AddDays(30),
                OTP = model.OTP,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { StatusCode = ResponseStatus.Failed, ResponseText = "User creation failed! Please check user details and try again." });
            #region send Notification


            var otpverify2 = await _userManager.ConfirmPhoneNumber(model.PhoneNumber, model.OTP, lockoutOnFailure: true);
            if (otpverify2.Succeeded)
            {
                _notify.SaveSMSEmailWhatsappNotification(new SMSEmailWhatsappNotification() { FormatID = MessageFormat.Registration, IsSms = true, IsWhatsapp = true, PhoneNumber = model.PhoneNumber, Name = model.Name, Password = model.Password }, User.GetLoggedInUserId<int>());
                return Ok(new Response { StatusCode = ResponseStatus.Success, ResponseText = "User created successfully!" });
            }
            else
            {
                return Ok(new Response { StatusCode = ResponseStatus.Failed, ResponseText = "Invalid OTP!" });
            }

            #endregion

        }
    }
}
