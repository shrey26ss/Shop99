using AppUtility.Helper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
                    sqlQuery = @"Update Category Set CategoryName=@CategoryName, ParentId=@ParentId,IsPublish=@ParentId,Icon=@Icon,ModifyBy=@LoginId,ModifyOn=GETDATE() where CategoryId = @CategoryId";
                }
                else
                {
                    sqlQuery = @"Insert into Category (CategoryName, ParentId,IsPublish,Icon,EntryOn,EntryBy)values(@CategoryName,@ParentId,@IsPublish,@Icon,GETDATE(),@LoginId);";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    category.Data.CategoryId,
                    category.Data.CategoryName,
                    category.Data.ParentId,
                    category.Data.IsPublish,
                    category.Data.Icon
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
                else
                {
                    res.ResponseText = description;
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
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Category(nolock) where Categoryid = @Id";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, new { request.Data.Id }, CommandType.Text);
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

        public async Task<Response<List<MenuItem>>> GetMenu(Request request)
        {
            var response = new Response<List<MenuItem>>() { Result = new List<MenuItem>() };
            var res = await Categories(request);
            var result = GenerateTreeWithRoot(res.Result);
            response.Result = result;
            return response;
        }
        private async Task<List<MenuItem>> GetChildrens(List<MenuItem> menu, List<MenuItem> MainMenu)
        {
            var res = new List<MenuItem>();
            foreach (var subChild in menu)
            {
                var subChilds = MainMenu.Where(a => a.ParentId == subChild.CategoryId).ToList();
                foreach(var childitem in subChilds)
                {
                    var subChild2 = MainMenu.Where(a => a.ParentId == childitem.CategoryId).ToList();
                    if(subChild2.Count > 0)
                    {
                       await GetChildrens(subChild2, MainMenu);
                    }
                }
            }
            return res;
        }
        private static List<MenuItem> GenerateTreeWithRoot(IEnumerable<Menu> collection)
        {
            List<MenuItem> lst = new List<MenuItem>();
            try
            {
                foreach (var rootItem in collection)
                {
                    if (rootItem.ParentId == 0)
                    {
                        var child = GenerateTree(collection, rootItem);
                    
                        lst.Add(new MenuItem { CategoryId = rootItem.CategoryId, CategoryName = rootItem.CategoryName, Children = child });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return lst;
        }
        private static List<MenuItem> GenerateTree(IEnumerable<Menu> collection,Menu rootItem)
        {
            List<MenuItem> lst = new List<MenuItem>();
            foreach (Menu c in collection.Where(c => c.ParentId == rootItem.CategoryId))
            {

                lst.Add(new MenuItem
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Children = GenerateTree(collection, c)
                });
            }
            return lst;
        }
        private async Task<Response<IEnumerable<Menu>>> Categories(Request request)
        {
            var res = new Response<IEnumerable<Menu>>();
            string sp = @"Select CategoryId,	CategoryName,	ParentId from Category(nolock) order by CategoryId";
            res.Result = await _dapper.GetAllAsync<Menu>(sp, null, CommandType.Text);
            return res;
        }

    }
}
