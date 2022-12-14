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

namespace Service.CatBrandMapping
{
    public class CategoryBrandMappingService : ICategoryBrandMapping
    {
        public readonly IDapperRepository _dapper;
        public readonly ILogger<CategoryBrandMappingService> _logger;
        public CategoryBrandMappingService(IDapperRepository dapper, ILogger<CategoryBrandMappingService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<CategoryBrandMapping> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "Proc_AddUpdateBrandCatMapping";
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    request.Data.CategoryId,
                    request.Data.BrandId
                }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<BrandMapping>>> GetMapped(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if (req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<BrandMapping>>();
            try
            {
                sp = @"Select b.[Name] BrandName, b.Id BrandId from BrandCategoryMapping(nolock) c inner join Brands(nolock) b on b.Id = c.BrandId inner join Category(nolock) ct on ct.CategoryId = c.CategoryId where c.CategoryId = @Id and c.IsActive = 1 order by b.[Name]";
                res.Result = await _dapper.GetAllAsync<BrandMapping>(sp, new { req.Data.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<IResponse<IEnumerable<BrandMapping>>> GetUnMapped(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if (req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<BrandMapping>>();
            try
            {
                sp = @"Select [Name] BrandName, Id BrandId from Brands(nolock) where Id not in (Select BrandId from BrandCategoryMapping(nolock) where CategoryId = @Id and IsActive = 1) order by [Name]";
                res.Result = await _dapper.GetAllAsync<BrandMapping>(sp, new { req.Data.Id }, CommandType.Text);
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
