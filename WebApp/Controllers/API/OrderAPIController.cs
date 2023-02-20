using AppUtility.Helper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using WebApp.Models.ViewModels;
using WebApp.AppCode;
using Microsoft.Extensions.Logging;
using WebApp.AppCode.Helper;
using WebApp.Models;
using Service.Models;
using Infrastructure.Interface;
using System.Net;

namespace WebApp.Controllers.API
{
    [ApiController]
    [Route("/api/")]
    public class OrderAPIController : ControllerBase
    {

        private readonly IHttpRequestInfo _httpInfo;
        public OrderAPIController(IHttpRequestInfo httpInfo)
        {
            _httpInfo = httpInfo;
        }
        [Route("OrderAPI/UploadReturnImage")]
        public async Task<IActionResult> UploadReturnImage([FromForm]ReplaceOrderImageVM req)
        {
            var res = await UploadImage(req.Files, req.OrderId);
            return Ok(res);
        }
        #region Private Methods
        public async Task<IResponse<string>> UploadImage(List<IFormFile> req, int OrderId)
        {
            var res = new Response<string>();
            if (OrderId == 0)
                return res;
            var ImagePath = new List<string>();
            if (req != null)
            {
                int counter = 0;
                foreach (var item in req)
                {
                    counter++;
                    string fileName = $"{counter.ToString() + DateTime.Now.ToString("ddMMyyyyhhmmssmmm")}.jpeg";
                    string filePath = FileDirectories.ReplaceOrderImage.Replace("{0}", OrderId.ToString()).Replace("//", "/");
                    var _ = Utility.O.UploadFile(new FileUploadModel
                    {
                        file = item,
                        FileName = fileName,
                        FilePath = filePath,
                        IsThumbnailRequired = false,
                    });
                    res.StatusCode = _.StatusCode; res.ResponseText = _.ResponseText;
                    ImagePath.Add(string.Concat(_httpInfo.AbsoluteURL() + "/", FileDirectories.ReplaceOrderImageSuffixDefault.Replace("{0}", OrderId.ToString()), fileName));
                }
            }
            res.Result = res.StatusCode == Entities.Enums.ResponseStatus.Success ? string.Join(',', ImagePath) : "";
            return res; 
        }
        #endregion
    }
}
