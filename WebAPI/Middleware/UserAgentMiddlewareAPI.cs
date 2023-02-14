using AppUtility.AppCode;
using AppUtility.Helper;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class UserAgentMiddlewareAPI
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAgentMiddlewareAPI(RequestDelegate next, IHttpContextAccessor httpContextAccessor) //OSInfo os,
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Invoke(HttpContext context)
        {
            OSInfoAPI oSInfo = new OSInfoAPI(_httpContextAccessor);
            if (string.IsNullOrEmpty(oSInfo.FullInfo))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("BadRequest");
            }
            await _next(context);
        }
    }
}
