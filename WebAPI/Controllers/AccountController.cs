using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Service.API;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models.ViewModels;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Services.Identity;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Variables
        private readonly ApplicationUserManager _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenService _tokenService;
        private IUserService _users;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;

        #endregion

        public AccountController(ApplicationUserManager userManager, ITokenService tokenService, ILogger<AccountController> logger, IUserService users, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenService = tokenService;
            _users = users;
            _configuration = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = _httpContextAccessor.HttpContext.RequestServices.GetService<SignInManager<ApplicationUser>>();
        }
        [AllowAnonymous]
        [HttpPost("/api/login")]
        public async Task<IActionResult> APILogin(LoginAPIRequest request)
        {
            var response = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid Login Attempt"
            };
            try
            {
                if (request is null)
                    return BadRequest("Invalid client request");
                var user = await _userManager.GetUserByToken(request);
                string token = string.Empty;
                if (user.Id > 0)
                {
                    if (!string.IsNullOrEmpty(user.Role) && !string.IsNullOrEmpty(user.UserName))
                    {
                        var claims = new[] {
                        new Claim(ClaimTypesExtension.Id, user.Id.ToString()),
                        new Claim(ClaimTypesExtension.Role, user.Role??"User"),
                        new Claim(ClaimTypesExtension.UserName, user.UserName),
                    };
                        token = _tokenService.GenerateAccessToken(claims);
                        var genrateRefreshToken = _tokenService.GenerateRefreshToken();
                        user.RefreshToken = genrateRefreshToken.RefreshToken;
                        user.RefreshTokenExpiryTime = genrateRefreshToken.RefreshTokenExpirationTime;
                        var res = await _userManager.UpdateAsync(user);
                    }
                    var authResponse = new AuthenticateResponse(user, token);
                    response = new Response<AuthenticateResponse>
                    {
                        StatusCode = ResponseStatus.Success,
                        ResponseText = ResponseStatus.Success.ToString(),
                        Result = authResponse
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Ok(response);
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
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists?.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { StatusCode = ResponseStatus.Failed, ResponseText = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                Role = Role.APIUser.ToString(),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { StatusCode = ResponseStatus.Failed, ResponseText = "User creation failed! Please check user details and try again." });

            return Ok(new Response { StatusCode = ResponseStatus.Success, ResponseText = "User created successfully!" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/loginnew")]
        public async Task<IActionResult> LoginNew(LoginViewModel model)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid Credentials"
            };
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.MobileNo);

                    //var claims = new[] {
                    //    new Claim(ClaimTypesExtension.Id, user.Id.ToString()),
                    //    new Claim(ClaimTypesExtension.Role, user.Role??"User"),
                    //    new Claim(ClaimTypesExtension.UserName, user.UserName),
                    //};
                    //var token = _tokenService.GenerateAccessToken(claims);
                    //var authResponse = new AuthenticateResponse(user, token);
                    //res.StatusCode = ResponseStatus.Success;
                    //res.ResponseText = "Login Succussful";
                    //res.Result = authResponse;

                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(res);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("/api/LoginNewtwo")]
        public async Task<IActionResult> LoginNewtwo(LoginViewModel model)
        {
            var res = new Response<AuthenticateResponse>
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid Credentials"
            };
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.MobileNo);

                    var claims = new[] {
                        new Claim(ClaimTypesExtension.Id, user.Id.ToString()),
                        new Claim(ClaimTypesExtension.Role, user.Role??"User"),
                        new Claim(ClaimTypesExtension.UserName, user.UserName),
                    };
                    var token = _tokenService.GenerateAccessToken(claims);
                    var authResponse = new AuthenticateResponse(user, token);
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Login Succussful";
                    res.Result = authResponse;
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(res);
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secretkey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("/api/GetUsers")]
        public async Task<IActionResult> Test()
        {
            var users = await _users.GetAllAsync(new ApplicationUser { Role = Role.Admin.ToString() }, 1);
            return Ok(users);
        }
    }
}
