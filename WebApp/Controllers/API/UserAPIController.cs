using AppUtility.Helper;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.AppCode;
using WebApp.Middleware;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers.API
{
    [ApiController]
    [Route("api/")]
    public class UserAPIController : ControllerBase
    {
        private readonly IHttpRequestInfo _httpInfo;
        public UserAPIController(IHttpRequestInfo httpInfo)
        {
            _httpInfo = httpInfo;
        }
        [Route("Uploadprofilepic")]
        public async Task<IActionResult> Uploadprofilepic([FromForm] UserAPIModel req)
        {
            var res = await UploadImage(req.Files, User.GetLoggedInUserId<int>() == 0? req.UserID: User.GetLoggedInUserId<int>());
            return Ok(res);
        }
        #region Private Methods
        public async Task<IResponse<string>> UploadImage(List<IFormFile> req, int UserID)
        {
            var res = new Response<string>();
            if (UserID == 0)
                return res;
            var ImagePath = new List<string>();
            if (req != null)
            {
                foreach (var item in req)
                {
                    string fileName = $"{Convert.ToString(UserID)}.jpeg";
                    string filePath = FileDirectories.Userpic;
                    var _ = Utility.O.UploadFile(new FileUploadModel
                    {
                        file = item,
                        FileName = fileName,
                        FilePath = filePath,
                        IsThumbnailRequired = false,
                    });
                    res.StatusCode = _.StatusCode; res.ResponseText = _.ResponseText;
                    ImagePath.Add(string.Concat(_httpInfo.AbsoluteURL() + "/", FileDirectories.UserpicSuffix, fileName));
                }
            }
            res.Result = res.StatusCode == Entities.Enums.ResponseStatus.Success ? string.Join(',', ImagePath) : "";
            return res;
        }
        #endregion
    }
}
