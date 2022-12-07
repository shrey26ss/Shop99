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
        public async Task<IResponse> AddUpdate(RequestBase<Brands> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update Brands Set Name=@Name,ModifyOn=GETDATE(),Ind=@Ind where Id = @Id";
                }
                else
                {
                    sqlQuery = @"Insert into Brands(Name,EntryOn,ModifyOn,Ind) values(@Name,GETDATE(),GETDATE(),@Ind)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Data.Name,
                    request.Data.Id,
                    request.Ind
                }, CommandType.Text);
                if (i > -1 && i < 100)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Brand add successfully";
                }
            }
            catch (Exception ex)
            {
               
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

            }
            return res;
        }
        public async Task<IResponse> AddUpdateBrandCategoryMapping(RequestBase<BrandCategoryMapping> request)
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
        public async Task<IResponse<IEnumerable<BrandCategoryMapping>>> GetBrandCategoryMapping(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<BrandCategoryMapping>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"select bcm.*,b.Name BrandName,c.CategoryName from BrandCategoryMapping bcm inner join Brands b on bcm.BrandId=b.Id inner join Category c on c.CategoryId=bcm.CategoryId where Id = @Id order by c.Ind,b.Ind";
                    res.Result = await _dapper.GetAllAsync<BrandCategoryMapping>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    sp = @"select bcm.*,b.Name BrandName,c.CategoryName from BrandCategoryMapping bcm inner join Brands b on bcm.BrandId=b.Id inner join Category c on c.CategoryId=bcm.CategoryId order by c.Ind,b.Ind";
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
        public async Task<IResponse<IEnumerable<BrandsDDL>>> GetBrandDDL()
        {
            string sp = @"Select Id,[Name] from Brands order by [Name]";
            var res = new Response<IEnumerable<BrandsDDL>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<BrandsDDL>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<IResponse<IEnumerable<Brands>>> GetTopBrands(int Top)
        {
            string sp = string.Empty;
            var res = new Response<IEnumerable<Brands>>();
            try
            {
                sp = @"Select Top(@Top) from Brands(nolock) order by Ind";
                res.Result = await _dapper.GetAllAsync<Brands>(sp, new { Top }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = ResponseStatus.Success.ToString();
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
