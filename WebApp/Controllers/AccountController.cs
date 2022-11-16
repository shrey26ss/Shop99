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

namespace WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        #region Variables
        private readonly JWTConfig _jwtConfig;
        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IUserService _users;
        private readonly ILogger<AccountController> _logger;
        //private readonly IRepository<EmailConfig> _emailConfig;
        private readonly ITokenService _tokenService;
        private IMapper _mapper;
        #endregion

        public AccountController(IOptions<JWTConfig> jwtConfig,
            ApplicationUserManager userManager, RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IUserService users, ITokenService tokenService,
            ILogger<AccountController> logger, IMapper mapper
            ) //IRepository<EmailConfig> emailConfig, _emailConfig = emailConfig;
        {
            _jwtConfig = jwtConfig.Value;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _users = users;
            _tokenService = tokenService;
        }

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Response response = new Response()
            {
                StatusCode = ResponseStatus.warning,
                ResponseText = "Registration Failed"
            };
            if (ModelState.IsValid)
            {
                var EmailId = model.Email;
                var user = new ApplicationUser
                {

                    UserId = Guid.NewGuid().ToString(),
                    UserName = model.Email.Trim(),
                    Email = model.Email.Trim(),
                    Role = Role.APIUser.ToString(),
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
                };
                var res = await _userManager.CreateAsync(user, model.Password);
                if (res.Succeeded)
                {
                    user = _userManager.FindByEmailAsync(user.Email).Result;
                    await _userManager.AddToRoleAsync(user, Role.APIUser.ToString());
                    model.Password = string.Empty;
                    model.Email = string.Empty;
                    response.StatusCode = ResponseStatus.Success;
                    response.ResponseText = "User Register Successfully";
                    ModelState.Clear();
                    //model = null;
                    model = new RegisterViewModel();
                }
                model.ResponseText = response.ResponseText;
                model.StatusCode = response.StatusCode;
                if (res.Errors?.Count() > 0)
                {
                    model.ResponseText = res.Errors.FirstOrDefault().Description;
                }
                return View(model);
            }
            return View(model);
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
                return View();
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    ReturnUrl = ReturnUrl?.Trim() == "/" ? "/Home/Index" : ReturnUrl;
                    return LocalRedirect(ReturnUrl);
                }
                else if (result.RequiresTwoFactor)
                {
                    if (!string.IsNullOrEmpty(model.GAuthPin))
                    {
                        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                        if (user == null)
                        {
                            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                        }
                        var results = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.GAuthPin, false, false);

                        if (results.Succeeded)
                        {
                            _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                            ReturnUrl = ReturnUrl?.Trim() == "/" ? "/dashboard" : ReturnUrl;
                            return LocalRedirect(ReturnUrl);
                        }
                        else
                        {
                            ModelState.Remove(string.Empty);
                            model = new LoginViewModel { IsTwoFactorEnabled = true };
                            ModelState.AddModelError(string.Empty, "Invalid Pin Entered");
                        }
                    }
                    else
                    {
                        ModelState.Remove(string.Empty);
                        model = new LoginViewModel { IsTwoFactorEnabled = true };
                        ModelState.AddModelError(string.Empty, "Google Authentication Pin Required");
                    }
                }
                else if (!result.Succeeded)
                {
                    ModelState.Remove(string.Empty);
                    ModelState.AddModelError(string.Empty, "Invalid email id or password");
                }
                else if (result.IsLockedOut)
                {
                    //ModelState.Remove(string.Empty);
                    //ModelState.AddModelError(string.Empty, "Your account is locked out.");
                    //var Scheme = Request.Scheme;
                    //var forgotPassLink = Url.Action(nameof(ForgotPassword), "Account", new { }, Request.Scheme);
                    //var content = string.Format("Your account is locked out, to reset your password, please click this link: {0}", forgotPassLink);
                    //var config = _emailConfig.GetAllAsync(new EmailConfig { Id = 2 }).Result;
                    //if (config == null || config.Count() == 0)
                    //    _logger.LogError("No Email configuration found", new { this.GetType().Name, fn = nameof(this.Login) });
                    //var setting = _mapper.Map<EmailSettings>(config?.Count() > 0 ? config.FirstOrDefault() : new EmailConfig());
                    //setting.Body = content;
                    //setting.Subject = "Locked out account information";
                    //var _ = AppUtility.O.SendMail(setting);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), new { this.GetType().Name, fn = nameof(this.Login) });
            }
            return View(model);
        }
        #endregion

    }
}
