using AppUtility.Helper;
using Dapper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async Task<IResponse> AddUpdate(RequestBase<Products> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sqlQuery = @"Update Products Set Name=@Name, Title=@Title,Description=@Description,SKU=@SKU,BrandId=@BrandId,CategoryId=@CategoryId,VendorId=@LoginId,ModifyBy=@LoginId,ModifyOn=GETDATE() where Id = @Id";
                }
                else
                {
                    sqlQuery = @"insert into Products (Name,Title,Description,SKU,BrandId,CategoryId,VendorId,EntryBy,ModifyBy,EntryOn,ModifyOn,ShortDescription) values(@Name,@Title,@Description,@SKU,@BrandId,@CategoryId,@LoginId,@LoginId,@LoginId,GETDATE(),GETDATE(),'ShortDescription')";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.LoginId,
                    request.RoleId,
                    request.Data.Id,
                    request.Data.Name,
                    request.Data.Title,
                    request.Data.Description,
                    request.Data.SKU,
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
        public async Task<IResponse<IEnumerable<Products>>> GetProducts(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Products>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select p.*, c.CategoryName,b.Name as BrandName from Products p inner join Category c on c.CategoryId = p.CategoryId inner join Brands b on b.Id = p.BrandId where c.Id = @Id ";
                    res.Result = await _dapper.GetAllAsync<Products>(sp, new { request.Data.Id, request.LoginId }, CommandType.Text);
                }
                else
                {
                    sp = @"Select p.*, c.CategoryName,b.Name as BrandName from Products p inner join Category c on c.CategoryId = p.CategoryId inner join Brands b on b.Id = p.BrandId order by [Name]";
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
        public async Task<IResponse> AddProductVariant(RequestBase<VariantCombination> request)
        {
            var res = new Response();
            try
            {
                var VariantGroup = ConvertToDataTable.ToDataTable(request.Data.GroupInfo);
                var AttributeInfo = ConvertToDataTable.ToDataTable(request.Data.AttributeInfo);
                var PictureInfo = ConvertToDataTable.ToDataTable(request.Data.PictureInfo);
                string sqlQuery = "Proc_AddVariant";
                int i = -5;
                DynamicParameters param = new DynamicParameters();
                param.Add("VariantGroup", VariantGroup, DbType.Object);
                param.Add("AttributeInfo", AttributeInfo, DbType.Object);
                param.Add("PictureInfo", PictureInfo, DbType.Object);
                param.Add("ProductId", request.Data.ProductId, DbType.Int32);
                param.Add("EntryBy", request.LoginId, DbType.Int32);
                i = await _dapper.GetByDynamicParamAsync<int>(sqlQuery, param, CommandType.StoredProcedure);
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
    }
}
