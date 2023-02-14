using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Net.Http.Headers;
using System.Linq;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using WebAPI.AppCode;
using AppUtility.AppCode;

namespace WebAPI.Middleware
{
    public class IpCheckRequirement : IAuthorizationRequirement
    {
        public bool IpClaimRequired { get; set; } = true;
    }
    public class IpCheckHandler : AuthorizationHandler<IpCheckRequirement>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        private HttpContext HttpContext => HttpContextAccessor.HttpContext;
        public IpCheckHandler(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IpCheckRequirement requirement)
        {
            string reqUA = HttpContext.Request.Headers["UserAgent"].ToString();            
            string reqIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            string authToken = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == HeaderNames.Authorization).ToString();
            string tokenIP = "";
            if (!string.IsNullOrEmpty(authToken) && authToken != "[, ]")
            {
                authToken = authToken.Split(",").Last();
                authToken = authToken.Split(" ").Last();
                authToken = authToken.Substring(0, authToken.Length - 1);

                if (!string.IsNullOrEmpty(authToken))
                {
                    //if (string.IsNullOrEmpty(reqUA))
                    //{
                    //    context.Fail();
                    //    return Task.CompletedTask;
                    //}
                    TokenSession printObj = JsonConvert.DeserializeObject<TokenSession>((new JwtSecurityTokenHandler().ReadToken(authToken) as JwtSecurityToken).ToString().Split(".").Last());
                    tokenIP = printObj.sameSession;
                }
            }
            if (authToken == "[, ]" && requirement.IpClaimRequired)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (tokenIP == EncodeString(reqIP + reqUA))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }


        private static string EncodeString(string serverName)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        }
        private class TokenSession
        {
            public string sameSession { get; set; }
        }

    }
}
