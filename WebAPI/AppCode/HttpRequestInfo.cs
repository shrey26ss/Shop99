using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.AppCode.Helper
{
    public interface IHttpRequestInfo
    {
        string AbsoluteURL();
    }
    public class HttpRequestInfo : IHttpRequestInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpRequestInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string AbsoluteURL()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            return string.Format("{0}://{1}", httpContext.Request.Scheme, httpContext.Request.Host);
        }
    }
}
