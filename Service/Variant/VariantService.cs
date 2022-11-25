using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Service.Variant
{
    public class VariantService : IVariant
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public VariantService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<Response> AddUpdate(RequestBase<ProductVariant> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update ProductVariant Set ProductId=@ProductId,AttributeId=@AttributeId,AttributeValue=@AttributeValue,Quantity=@Quantity,MRP=@MRP,ModifyOn=GETDATE(),ModifyBy=@LoginId,Ind=@Ind where Id = @Id";
                }
                else
                {
                    sqlQuery = @"Insert into ProductVariant(ProductId,AttributeId,AttributeValue,Quantity,MRP,EntryBy,EntryOn,Ind,ModifyOn,ModifyBy) values(@ProductId,@AttributeId,@AttributeValue,@Quantity,@MRP,@LoginId,GETDATE(),GETDATE(),@LoginId)";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.RoleId,
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

        public async Task<Response<IEnumerable<ProductVariant>>> GetVariants(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<ProductVariant>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from ProductVariant(nolock) where Id = @Id and EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<ProductVariant>(sp, new { request.Data.Id, request.LoginId }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from ProductVariant(nolock) where EntryBy = @LoginId order by Ind";
                    res.Result = await _dapper.GetAllAsync<ProductVariant>(sp, new { request.LoginId }, CommandType.Text);
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
