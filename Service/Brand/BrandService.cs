using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DapperRepository> _logger;
        public BrandService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<Response> AddUpdate(RequestBase<Brands> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update Brands Set CategoryId=@CategoryId,Name=@Name,Description=@Description,ModifyOn=GETDATE(),ModifyBy=@LoginId where Id = @Id";
                }
                else
                {
                    sqlQuery = @"Insert into Brands(CategoryId,Name,Description,EntryOn,EntryBy) values(@CategoryId,@Name,@Description,GETDATE(),@LoginId)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.RoleId,
                    request.Data.CategoryId,
                    request.Data.Name,
                    request.Data.Description,
                    request.Data.Id
                }, CommandType.Text);
                if (i > -1 && i < 100)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
            }

            return res;
        }

        public async Task<Response<IEnumerable<Brands>>> GetBrands(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Brands>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Brands(nolock) where Id = @Id and EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Brands>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from Brands(nolock) where EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Brands>(sp, new { request.LoginId}, CommandType.Text);
                }
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<Response> AddUpdateBrandCategoryMapping(RequestBase<BrandCategoryMapping> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update BrandCategoryMapping Set BrandId=@BrandId,CategoryId=@CategoryId where Id=@Id";
                }
                else
                {
                    sqlQuery = @"insert into BrandCategoryMapping (BrandId,CategoryId) values(@BrandId,@CategoryId)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.RoleId,
                    request.Data.CategoryId,
                    request.Data.Id,
                    request.Data.BrandId
                }, CommandType.Text);
                if (i > -1 && i < 100)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
            }

            return res;
        }
        public async Task<Response<IEnumerable<BrandCategoryMapping>>> GetBrandCategoryMapping(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<BrandCategoryMapping>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from BrandCategoryMapping(nolock) where Id = @Id";
                    res.Result = await _dapper.GetAllAsync<BrandCategoryMapping>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from BrandCategoryMapping(nolock)";
                    res.Result = await _dapper.GetAllAsync<BrandCategoryMapping>(sp, null, CommandType.Text);
                }
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
