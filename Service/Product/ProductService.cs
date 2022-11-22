using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Product
{
    public class ProductService : IProducts
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public ProductService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<Response> AddUpdate(RequestBase<Products> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update Products Set Specification=@Specification,Description=@Description,BrandId=@BrandId,CategoryId=@CategoryId,VendorId=@VendorId,ModifyOn=GETDATE(),ModifyBy=@LoginId where Id = @Id";
                }
                else
                {
                    sqlQuery = @"insert into Products (Specification,Description,BrandId,CategoryId,VendorId,EntryBy,EntryOn) values(@Specification,@Description,@BrandId,@CategoryId,@VendorId,@LoginId,Getdate())";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.RoleId,
                    request.Data.Id,
                    request.Data.Specification,
                    request.Data.Description,
                    request.Data.BrandId,
                    request.Data.CategoryId,
                    request.Data.VendorId
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

        public async Task<Response<IEnumerable<Products>>> GetProducts(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Products>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Products(nolock) where Id = @Id and EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Products>(sp, new { request.Data.Id,request.LoginId }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from Products(nolock) where EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Products>(sp, new { request.LoginId }, CommandType.Text);
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
