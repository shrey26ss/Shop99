using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Net.Http.Headers;
using System.Linq;

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
            Claim ipClaim = context.User.FindFirst(claim => claim.Type == HeaderNames.Authorization);
            //var authorization = context.User.Claims.GetType(""); as ClaimsPrincipal;//.Headers[HeaderNames.Authorization];
            string reqIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            string authToken = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == HeaderNames.Authorization).ToString() ?? "";
            var encodedIPAgent = context.User.Claims.FirstOrDefault(a => a.Type == "sameSession");
            // No claim existing set and and its configured as optional so skip the check
            if (ipClaim == null && !requirement.IpClaimRequired)
            {
                // Optional claims (IsClaimRequired=false and no "ipaddress" in the claims principal) won't call context.Fail()
                // This allows next Handle to succeed. If we call Fail() the access will be denied, even if handlers
                // evaluated after this one do succeed
                return Task.CompletedTask;
            }

            //if (ipClaim.Value = HttpContext.Connection.RemoteIpAddress?.ToString())
            //{
            //    context.Succeed(requirement);
            //}
            //else
            //{
            //    // Only call fail, to guarantee a failure, even if further handlers may succeed
            //    context.Fail();
            //}

            return Task.CompletedTask;
        }


        private static string DecodeString(string encodedServername)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
        }
        private class TokenSession
        {
            public string sameSession { get; set; }
        }

    }
}
