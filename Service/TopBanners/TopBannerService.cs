﻿using AppUtility.Helper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.TopBanners
{
    public class TopBannerService : ITopBanner
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public TopBannerService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<TopBanner> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update TopBanner Set BannerPath=@BannerPath,Title=@Title,Subtitle=@Subtitle,BackLinkText=@BackLinkText,BackLinkURL=@BackLinkURL,EntryBy=@LoginId,ModifyBy=@LoginId,ModifyOn=GETDATE() where Id =@Id";
                }
                else
                {
                    sqlQuery = @"Insert into TopBanner (BannerPath,Title,Subtitle,BackLinkText,BackLinkURL,EntryBy,ModifyBy,EntryOn,ModifyOn) values(@BannerPath,@Title,@Subtitle,@BackLinkText,@BackLinkURL,@LoginId,@LoginId,GETDATE(),GETDATE())";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.Data.Id,
                    request.Data.Title,
                    request.Data.BannerPath,
                    request.Data.Subtitle,
                    request.Data.BackLinkText,
                    request.Data.BackLinkURL
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
            }

            return res;
        }
        public async Task<IResponse<IEnumerable<TopBanner>>> GetDetails(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if(req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<TopBanner>>();
            try
            {
                sp = @"Select * from TopBanner(nolock) where Id = @Id or Isnull(@Id,0)=0 Order by Id Desc";
                res.Result = await _dapper.GetAllAsync<TopBanner>(sp, new { req.Data.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
