using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Service.API;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using AppUtility.AppCode;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApp.Middleware
{
    public class UserAgentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAgentMiddleware(RequestDelegate next,  IHttpContextAccessor httpContextAccessor) //OSInfo os,
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Invoke(HttpContext context)
        {
            OSInfo oSInfo = new OSInfo(_httpContextAccessor);
            if (string.IsNullOrEmpty(oSInfo.FullInfo))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("BadRequest");
            }
            await _next(context);
        }

        //private void attachUserToContext(HttpContext context, string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = Encoding.ASCII.GetBytes(_osInfo.Secretkey);
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            //ClockSkew = TimeSpan.Zero // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //        }, out SecurityToken validatedToken);

        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var claims = jwtToken.Claims.ToList();
        //        var loginResponse = new LoginResponse
        //        {
        //            StatusCode = ResponseStatus.Success,
        //            ResponseText = nameof(ResponseStatus.Success),
        //            IsAuthenticate = true,
        //            Token = token,
        //            Result = new ApplicationUser
        //            {
        //                Id = int.Parse(claims.First(x => x.Type == "id").Value),
        //                Role = Convert.ToString(claims.First(x => x.Type == "role").Value),
        //                UserName = Convert.ToString(claims.First(x => x.Type == "userName").Value)
        //            }
        //        };
        //        context.Items["User"] = loginResponse;
        //    }
        //    catch
        //    {
        //        // do nothing if jwt validation fails
        //        // user is not attached to context so request won't have access to secure routes
        //    }
        //}
    }
}
