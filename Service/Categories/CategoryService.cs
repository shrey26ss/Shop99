using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Service.Categories
{
    public class CategoryService : ICategoryService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public CategoryService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<Response> AddUpdate(RequestBase<Category> category)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (category.Data.CategoryId != 0 && category.Data.CategoryId > 0)
                {
                    sqlQuery = @"Update Category(nolock) Set CategoryName=@CategoryName, ParentId=@ParentId,IsPublish=@ParentId,Icon=@Icon,ModifyBy=@LoginId,ModifyOn=GETDATE() where CategoryId = @CategoryId";
                }
                else
                {
                    sqlQuery = @"Insert into Category(nolock) (CategoryName, ParentId,IsPublish,Icon,EntryOn,EntryBy)values(@CategoryName,@ParentId,@IsPublish,@Icon,GETDATE(),@LoginId);";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    category.LoginId,
                    category.RoleId,
                    category.Data.CategoryId,
                    category.Data.CategoryName,
                    category.Data.ParentId,
                    category.Data.IsPublish,
                    category.Data.Icon
                }, CommandType.Text);
                if (i > -1)
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

        public async Task<Response> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<IEnumerable<Category>>> GetCategories(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Category>>();
            try
            {
                if(request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Category(nolock) where Categoryid = @Id and EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, new { request.Data.Id, request.LoginId}, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from Category(nolock) where EntryBy = @LoginId";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, new {request.LoginId}, CommandType.Text);
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
