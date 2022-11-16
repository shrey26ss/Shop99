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

        #endregion

        public AccountController(ApplicationUserManager userManager, ITokenService tokenService, ILogger<AccountController> logger, IUserService users)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenService = tokenService;
            _users = users;
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
        [JWTAuthorize]
        [HttpPost("/api/GetUsers")]
        public async Task<IActionResult> GetUserList()
        {
            var users = await _users.GetAllAsync(new ApplicationUser { Role = Role.Admin.ToString() }, 1);
            return Ok(users);
        }
    }
}
