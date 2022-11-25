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

namespace Service.Attribute
{
    public class AttributeService : IAttributes
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public AttributeService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<Response> AddUpdate(RequestBase<Attributes> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update Attributes Set Name=@Name,Value=@Value,ModifyOn=GETDATE(),Ind=@Ind where Id = @Id";
                }
                else
                {
                    sqlQuery = @"Insert into Attributes (Name,Value,EntryOn,Ind) values(@Name,@Value,GETDATE(),@Ind)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Ind,
                    request.RoleId,
                    request.Data.Name,
                    request.Data.Id,
                    request.Data.Value
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

        public async Task<Response<IEnumerable<Attributes>>> GetAttributes(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Attributes>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Attributes(nolock) where Id = @Id and EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Attributes>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from Attributes(nolock) order by Ind";
                    res.Result = await _dapper.GetAllAsync<Attributes>(sp, new { }, CommandType.Text);
                }
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<Response<IEnumerable<AttributesDDL>>> GetAttributeDDL()
        {
            string sp = @"Select Id, [Name] from Attributes Order By [Name]";
            var res = new Response<IEnumerable<AttributesDDL>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<AttributesDDL>(sp, new { }, CommandType.Text);
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
