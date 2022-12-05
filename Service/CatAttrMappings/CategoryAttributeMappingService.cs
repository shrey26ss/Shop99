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
using AppUtility.Helper;

namespace Service.CatAttrMappings
{
    public class CategoryAttributeMappingService : ICategoryAttributeMapping
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public CategoryAttributeMappingService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse> AddUpdate(RequestBase<CategoryAttrMapping> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "Proc_AddUpdateCatAttrMapping";
                res = await _dapper.GetAsync<Response>(sqlQuery, new
                {
                    request.Data.CategoryId,
                    request.Data.AttributeId
                }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<AtttributeMapping>>> GetMapped(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if (req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<AtttributeMapping>>();
            try
            {
                sp = @"Select a.[Name] AttributeName, a.Id AttributeId from CategoryAttributeMapping(nolock) c inner join Attributes(nolock) a on a.Id = c.AttributeId inner join Category(nolock) ct on ct.CategoryId = c.CategoryId where c.CategoryId = @Id and c.IsActive = 1 order by a.[Name]";
                res.Result = await _dapper.GetAllAsync<AtttributeMapping>(sp, new { req.Data.Id }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        } 
        public async Task<IResponse<IEnumerable<AtttributeMapping>>> GetUnMapped(RequestBase<SearchItem> req)
        {
            string sp = string.Empty;
            if (req.Data == null)
                req.Data = new SearchItem();
            var res = new Response<IEnumerable<AtttributeMapping>>();
            try
            {
                sp = @"Select [Name] AttributeName, Id AttributeId from Attributes(nolock) where Id not in (Select AttributeId from CategoryAttributeMapping(nolock) where CategoryId = @Id and IsActive = 1) order by [Name]";
                res.Result = await _dapper.GetAllAsync<AtttributeMapping>(sp, new { req.Data.Id }, CommandType.Text);
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
