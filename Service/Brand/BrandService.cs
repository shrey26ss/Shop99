using AppUtility.Helper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using NLog;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.Brand
{
    public class BrandService : IBrands
    {
        private IDapperRepository _dapper;
        private readonly ILogger<BrandService> _logger;
        public BrandService(IDapperRepository dapper, ILogger<BrandService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<Brands> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update Brands Set Name=@Name,ModifyOn=GETDATE(),Ind=@Ind where Id = @Id; Select 1 StatusCode, @Id ResponseText";
                }
                else
                {
                    sqlQuery = @"Insert into Brands(Name,EntryOn,ModifyOn,Ind,IsPublished) values(@Name,GETDATE(),GETDATE(),@Ind,1); Select 1 StatusCode, SCOPE_IDENTITY() ResponseText";
                }
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    request.Data.Name,
                    request.Data.Id,
                    request.Ind,
                    request.Data.IsPublished
                }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return res;
        }
        public async Task<IResponse<IEnumerable<Brands>>> GetBrands(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Brands>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Brands(nolock) where Id = @Id";
                    res.Result = await _dapper.GetAllAsync<Brands>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from Brands(nolock) order by Ind";
                    res.Result = await _dapper.GetAllAsync<Brands>(sp, new { }, CommandType.Text);
                }
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<BrandsDDL>>> GetBrandDDL()
        {
            string sp = @"Select Id,[Name] from Brands(nolock) order by [Name]";
            var res = new Response<IEnumerable<BrandsDDL>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<BrandsDDL>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<Brands>>> GetTopBrands(int Top)
        {
            string sp = string.Empty;
            var res = new Response<IEnumerable<Brands>>();
            try
            {
                sp = @"Select Top(@Top) * from Brands(nolock) Where IsPublished = 1 order by Ind";
                res.Result = await _dapper.GetAllAsync<Brands>(sp, new { Top }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = ResponseStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse> UpdateIspublishBrands(RequestBase<UpdateIspublishBrands> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;

                sqlQuery = @"Update Brands set IsPublished = @IsPublish where Id = @Id;";

                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Data.Id,
                    request.Data.IsPublish
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;

                    res.ResponseText = "Brand Updated successfully";
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
