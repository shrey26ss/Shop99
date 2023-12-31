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

namespace Service.TopLowerBanners
{
    public class TopLowerBannerService : ITopLowerBanner
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public TopLowerBannerService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<TopLowerBanner> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update TopLowerBanner Set BannerPath=@BannerPath,Title=@Title,Subtitle=@Subtitle,BackLinkText=@BackLinkText,BackLinkURL=@BackLinkURL,EntryBy=@LoginId,ModifyBy=@LoginId,ModifyOn=GETDATE() where Id =@Id";
                }
                else
                {
                    sqlQuery = @"Insert into TopLowerBanner (BannerPath,Title,Subtitle,BackLinkText,BackLinkURL,EntryBy,ModifyBy,EntryOn,ModifyOn) values(@BannerPath,@Title,@Subtitle,@BackLinkText,@BackLinkURL,@LoginId,@LoginId,GETDATE(),GETDATE())";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.Data.Id,
                    Title = request.Data.Title ?? "",
                    BannerPath = request.Data.BannerPath,
                    Subtitle= request.Data.Subtitle ?? "",
                    BackLinkText = request.Data.BackLinkText ?? "",
                    BackLinkURL = request.Data.BackLinkURL ?? ""
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

        public async Task<IResponse<IEnumerable<TopLowerBanner>>> GetDetails(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if (req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<TopLowerBanner>>();
            try
            {
                sp = @"Select * from TopLowerBanner(nolock) where Id = @Id or Isnull(@Id,0)=0 Order by Id Desc";
                res.Result = await _dapper.GetAllAsync<TopLowerBanner>(sp, new { req.Data.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<IResponse> Delete(RequestBase<SearchItem> req)
        {
            var res = new Response();
            try
            {
                if (req.Data == null)
                    return res;
                string sqlQuery = @"Delete from TopLowerBanner where Id = @Id";
                int i = -5;
                i = await _dapper.ExecuteAsync(sqlQuery, new { req.Data.Id }, CommandType.Text);
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
    }
}
