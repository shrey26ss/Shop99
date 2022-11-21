using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
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
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select * from Category(nolock) where Categoryid = @Id";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    sp = @"Select * from Category(nolock)";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, null, CommandType.Text);
                }
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public async Task<Response<Menu>> GetMenu(Request request)
        {
            var response = new Response<Menu>() { Result = new Menu() };
            var res = await Categories(request);
            var resssss = GetMenu2(res.Result.ToList());
            return response;
        }
        private async Task<List<Menu>> GetMenu2(List<Menu> mainMenu)
        {
            var response = new List<Menu>();
            var res = mainMenu;
            var rootList = new List<Menu>();
            //Root menu
            var rootmenu = res.Where(a => a.ParentId == 0).ToList();
            // Child Menu
            foreach (var parent in rootmenu)
            {
                var childMenu = res.Where(a => a.ParentId == parent.CategoryId).ToList();
                response.Add(parent);
                if (childMenu.Count > 0)
                {
                    parent.CategoryList = await GetChildrens(childMenu, res.ToList());
                }
                response.Add(parent);
            }
            return response;
        }
        private async Task<List<Menu>> GetChildrens(List<Menu> menu, List<Menu> MainMenu)
        {
            var res = new List<Menu>();
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
        private async Task<Response<IEnumerable<Menu>>> Categories(Request request)
        {
            string sp = string.Empty;
            var res = new Response<IEnumerable<Menu>>();
            sp = @"Select * from Category(nolock)";
            res.Result = await _dapper.GetAllAsync<Menu>(sp, null, CommandType.Text);
            return res;
        }

    }
}
