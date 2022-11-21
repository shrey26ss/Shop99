using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Linq;

namespace Service.API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JWTAuthorizeAttribute : System.Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isValid = false;

            IServiceProvider services = context.HttpContext.RequestServices;

            JWTConfig _jwtConfig = services.GetService<JWTConfig>();
            string XAuth = context.HttpContext.Request.Headers["X-Auth"];
            XAuth = XAuth ?? string.Empty;
            if (XAuth.StartsWith("."))
            {
                XAuth = XAuth.Remove(0, 1);
            }
            if (!string.IsNullOrEmpty(XAuth))
            {
                isValid = attachUserToContext(context.HttpContext, XAuth, _jwtConfig);
            }
            if (!isValid)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

        private bool attachUserToContext(HttpContext context, string token, JWTConfig jwtConfig)
        {
            bool isValid = false;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtConfig.Secretkey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ClockSkew = TimeSpan.Zero // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToList();
                var loginResponse = new LoginResponse
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = nameof(ResponseStatus.Success),
                    IsAuthenticate = true,
                    Token = token,
                    Result = new ApplicationUser
                    {
                        Id = int.Parse(claims.First(x => x.Type == ClaimTypesExtension.Id).Value),
                        Role = Convert.ToString(claims.First(x => x.Type == ClaimTypesExtension.Role).Value),
                        UserName = Convert.ToString(claims.First(x => x.Type == ClaimTypesExtension.UserName).Value)
                    }
                };
                context.Items["User"] = loginResponse;
                isValid = true;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }

            return isValid;
        }
    }
}
