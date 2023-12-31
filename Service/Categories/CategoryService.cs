﻿using AppUtility.Helper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public async Task<IResponse> AddUpdate(RequestBase<Category> category)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                if (category.Data.CategoryId != 0 && category.Data.CategoryId > 0)
                {
                    sqlQuery = @"Update Category Set CategoryName=@CategoryName,TAXRate=@TAXRate, ParentId=@ParentId,IsPublish=@IsPublish,Icon=@Icon,ModifyOn=GETDATE(),Ind=@Ind where CategoryId = @CategoryId";
                }
                else
                {
                    sqlQuery = @"Insert into Category (CategoryName, ParentId,IsPublish,Icon,EntryOn,ModifyOn,Ind,IsVendorGrouped,TAXRate)values(@CategoryName,@ParentId,@IsPublish,@Icon,GETDATE(),GETDATE(),0,@IsVendorGrouped,@TAXRate);";
                }
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    category.Data.CategoryId,
                    category.Data.CategoryName,
                    category.Data.ParentId,
                    category.Data.TAXRate,
                    category.Data.IsPublish,
                    Icon = category.Data.Icon ?? "",
                    category.Ind,
                    IsVendorGrouped = category.Data.IsVendorGrouped is null ? true : category.Data.IsVendorGrouped
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Category add successfully";
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }



        public async Task<IResponse> UpdateIsPublishCategory(RequestBase<CategoryIsPublishUpdate> category)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
               
                    sqlQuery = @"update Category set IsPublish = @IsPublish where CategoryId = @ParentId;";
              
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    category.Data.ParentId,
                    category.Data.IsPublish
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "Category Updated successfully";
                }
                else
                {
                    res.ResponseText = description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }


        public async Task<IResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<IEnumerable<Category>>> GetCategories(RequestBase<SearchItem> request)
        {
            string sp = string.Empty;
            if (request.Data == null)
                request.Data = new SearchItem();
            var res = new Response<IEnumerable<Category>>();
            try
            {
                if (request.Data.Id != 0 && request.Data.Id > 0)
                {
                    sp = @"Select c.*, p.CategoryName as ParentName from Category(nolock) c Left join Category p(nolock) on p.CategoryId = c.ParentId where c.CategoryId = @Id";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, new { request.Data.Id }, CommandType.Text);
                }
                else
                {
                    //sp = @"Select c.*, p.CategoryName as ParentName from Category(nolock) c inner join Category p on p.CategoryId = c.ParentId Order by c.Ind";
                    sp = @"Select c.*, p.CategoryName as ParentName from Category(nolock) c Left join Category p(nolock) on p.CategoryId = c.ParentId Order by c.Ind";
                    res.Result = await _dapper.GetAllAsync<Category>(sp, new { }, CommandType.Text);
                }
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<Category>>> TopCategories(TopCategoryRequest request)
        {
            string sp = @";with cte as(
                                            select  ProductId from VariantGroup  group by ProductId)
                                            select  top(@Top) c.CategoryId,c.CategoryName,c.Icon 
                                            from    cte inner join Products p on p.Id=cte.ProductId
                                                    inner join Category c on c.CategoryId=p.CategoryId group by  c.CategoryId,c.CategoryName,c.Icon";
            if (request.Top == -1)
            {
                sp = @";with cte as(
                                        select  ProductId from VariantGroup  group by ProductId)
                                        select  c.CategoryId,c.CategoryName,c.Icon 
                                        from    cte inner join Products p on p.Id=cte.ProductId
                                                inner join Category c on c.CategoryId=p.CategoryId group by  c.CategoryId,c.CategoryName,c.Icon";
            }
            var res = new Response<IEnumerable<Category>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<Category>(sp, new { request.Top }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public async Task<IResponse<List<MenuItem>>> GetMenu(Request request)
        {
            var response = new Response<List<MenuItem>>()
            {
                StatusCode = ResponseStatus.Success,
                ResponseText = ResponseStatus.Success.ToString(),
                Result = new List<MenuItem>()
            };
            var res = await Categories();
            response.Result = GenerateTreeWithRoot(res.Result);
            return response;
        }

        public async Task<IResponse<IEnumerable<CategoryDDL>>> GetCategoriesDDL()
        {
            string sp = @"Select CategoryId, CategoryName from Category(nolock) where IsPublish = 1 order by CategoryName";
            var res = new Response<IEnumerable<CategoryDDL>>();
            try
            {
                res.Result = await _dapper.GetAllAsync<CategoryDDL>(sp, new { }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {

            }
            return res;
        }


        #region Private Method
        private List<MenuItem> GenerateTreeWithRoot(IEnumerable<Category> collection)
        {
            List<MenuItem> lst = new List<MenuItem>();
            try
            {
                foreach (var rootItem in collection)
                {
                    if (rootItem.ParentId == 0)
                    {
                        var child = GenerateTree(collection, rootItem);

                        lst.Add(new MenuItem
                        {
                            CategoryId = rootItem.CategoryId,
                            CategoryName = rootItem.CategoryName,
                            ParentId = rootItem.ParentId,
                            Icon = rootItem.Icon,
                            IsPublish = rootItem.IsPublish,
                            ChildNode = child
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return lst;
        }
        private List<MenuItem> GenerateTree(IEnumerable<Category> collection, Category rootItem)
        {
            List<MenuItem> lst = new List<MenuItem>();
            foreach (Category c in collection.Where(c => c.ParentId == rootItem.CategoryId))
            {
                lst.Add(new MenuItem
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    IsPublish = c.IsPublish,
                    Icon = c.Icon,
                    ParentId = c.ParentId,
                    ChildNode = GenerateTree(collection, c)
                });
            }
            return lst;
        }
        private async Task<IResponse<IEnumerable<Category>>> Categories()
        {
            var res = new Response<IEnumerable<Category>>();
            string sp = @"Select CategoryId,CategoryName,ParentId,Icon from Category(nolock) Where IsPublish = 1 order by Ind";
            res.Result = await _dapper.GetAllAsync<Category>(sp, null, CommandType.Text);
            return res;
        }


        #endregion


    }
}
