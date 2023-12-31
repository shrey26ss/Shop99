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
namespace Service.Websiteinfo
{
    public class WebsiteinfoService: IWebsiteinfo
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public WebsiteinfoService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<WebsiteinfoModel> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    if (string.IsNullOrEmpty(request.Data.Whitelogo) && !string.IsNullOrEmpty(request.Data.Coloredlogo))
                    {
                        sqlQuery = @"Update CompanyProfile set Coloredlogo=@Coloredlogo,CompanyDomain=@Companydomain,CompanyName=@Companyname,CompanyEmailID=@CompanyemailID,CompanyMobile=@Companymobile,CompanyAddress=@Companyaddress,Footerdescription=@Footerdescription,ModifyOn=GETDATE() where ID = @Id";
                    }
                    else if(string.IsNullOrEmpty(request.Data.Coloredlogo) && !string.IsNullOrEmpty(request.Data.Whitelogo))
                    {
                        sqlQuery = @"Update CompanyProfile set Whitelogo = @Whitelogo,CompanyDomain=@Companydomain,CompanyName=@Companyname,CompanyEmailID=@CompanyemailID,CompanyMobile=@Companymobile,CompanyAddress=@Companyaddress,Footerdescription=@Footerdescription,ModifyOn=GETDATE() where ID = @Id";
                    }
                    else if (!string.IsNullOrEmpty(request.Data.Coloredlogo) || !string.IsNullOrEmpty(request.Data.Whitelogo))
                    {
                        sqlQuery = @"Update CompanyProfile set Whitelogo = @Whitelogo,Coloredlogo=@Coloredlogo,CompanyDomain=@Companydomain,CompanyName=@Companyname,CompanyEmailID=@CompanyemailID,CompanyMobile=@Companymobile,CompanyAddress=@Companyaddress,Footerdescription=@Footerdescription,ModifyOn=GETDATE() where ID = @Id";
                    }
                    else
                    {
                        sqlQuery = @"Update CompanyProfile set CompanyDomain=@Companydomain,CompanyName=@Companyname,CompanyEmailID=@CompanyemailID,CompanyMobile=@Companymobile,CompanyAddress=@Companyaddress,Footerdescription=@Footerdescription,ModifyOn=GETDATE() where ID = @Id";
                    }
                    
                }
                else
                {
                    sqlQuery = @"insert into CompanyProfile(Whitelogo,Coloredlogo,CompanyDomain,CompanyName,CompanyEmailID,CompanyMobile,CompanyAddress,Footerdescription,EnteryOn,ModifyOn)
values(@Whitelogo,@Coloredlogo,@Companydomain,@Companyname,@CompanyemailID,@Companymobile,@Companyaddress,@Footerdescription,GETDATE(),GETDATE());";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.Data.Id,
                    request.Data.Whitelogo,
                    request.Data.Coloredlogo,
                    request.Data.Companydomain,
                    request.Data.Companyname,
                    request.Data.CompanyemailID,
                    request.Data.Companymobile,
                    request.Data.Companyaddress,
                    request.Data.Footerdescription,
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
        public async Task<IResponse<IEnumerable<WebsiteinfoModel>>> GetDetails(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if (req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<WebsiteinfoModel>>();
            try
            {
                sp = @"Select * from CompanyProfile(nolock) where Id = @Id or Isnull(@Id,0)=0 Order by Id Desc";
                res.Result = await _dapper.GetAllAsync<WebsiteinfoModel>(sp, new { req.Data.Id }, CommandType.Text);
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
                string sqlQuery = @"Delete from CompanyProfile where Id = @Id";
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
