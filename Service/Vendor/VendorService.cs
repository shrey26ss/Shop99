using AppUtility.Helper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.Vendor
{
    public class VendorService : IVendor
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public VendorService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IResponse<ValidateVendor>> ValidateVendor(RequestBase<SearchItem> request)
        {
            var res = new Response<ValidateVendor>();
            try
            {
                string sqlQuery = "Proc_ValidateVendor";
                res.Result = await _dapper.GetAsync<ValidateVendor>(sqlQuery, new { request.LoginId }, CommandType.StoredProcedure);
                if (res.Result.IsOnboard)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = res.Result.ResponseText;
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }
        public async Task<IResponse> AddUpdate(RequestBase<VendorProfile> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update VendorProfile Set UserId=@LoginId,StateId = @StateId,ShopName=@ShopName,GSTNumber=@GSTNumber,TIN=@TIN,Address=@Address,ContactNo=@ContactNo,ModifyBy=@LoginId,ModifyOn=Getdate() where Id = @Id";
                }
                else
                {
                    sqlQuery = @"insert into VendorProfile (UserId,ShopName,GSTNumber,TIN,Address,ContactNo,EntryBy,ModifyBy,EntryOn,ModifyOn,StateId)values(@LoginId,@ShopName,@GSTNumber,@TIN,@Address,@ContactNo,@LoginId,@LoginId,Getdate(),Getdate(),@StateId)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.Data.Id,
                    request.Data.ShopName,
                    request.Data.GSTNumber,
                    TIN = request.Data.TIN ?? string.Empty,
                    request.Data.Address,
                    request.Data.ContactNo,
                    request.Data.StateId
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

        public async Task<IResponse<VendorProfile>> GetVendorDetails(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            var res = new Response<VendorProfile>();
            try
            {
                sp = @"Select * from VendorProfile(nolock) where UserId = @LoginId";
                res.Result = await _dapper.GetAsync<VendorProfile>(sp, new { request.LoginId }, CommandType.Text);
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
