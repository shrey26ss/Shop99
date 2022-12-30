using Dapper;
using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Service.Homepage
{
    public class HomepageService : IHomepageService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public HomepageService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetProductByCategory(ProductRequest<CategorFilter> productRequest)
            {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                //    string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
                //inner join VariantGroup vg on vg.ProductId = p.Id 
                //where p.CategoryId = @CategoryId and vg.IsShowOnHome=1 ";
                string sqlQuery = @"CREATE TABLE #temp (attributes varchar(max)) 
insert into #temp select * from  dbo.fn_SplitString(@Attributes,',')
  if((select count(*) from #temp)>0)
  begin
   with cte as ( Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
			 inner join CategoryAttributeMapping cam on cam.CategoryId=p.CategoryId
			  inner join AttributeInfo ai on ai.AttributeId=cam.AttributeId
			 inner join #temp t on t.attributes = ai.AttributeValue and vg.id=ai.GroupId
            where p.CategoryId = @CategoryId and vg.IsShowOnHome=1
)select * from cte group by 	ProductID,VariantID,PublishedOn,Title,MRP,GroupID,ImagePath,Label,SellingCost,Stars
  end
  else
  begin
   Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
            where p.CategoryId =@CategoryId and vg.IsShowOnHome=1
  end";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { productRequest.MoreFilters.CategoryId, productRequest.MoreFilters.Attributes, Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetRandomProduct(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id
            where vg.IsShowOnHome = 1 order by NEWID() desc ";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetNewArrivals(ProductRequest<int> productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
            where vg.IsShowOnHome=1 and DATEDIFF(D,vg.PublishedOn,getdate())<=@days order by p.ID desc ";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { days = productRequest.MoreFilters < 1 ? 5 : productRequest.MoreFilters, Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetFeaturedProduct(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
            where vg.IsFeatured=1";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetBestSellerProduct(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
             order by vg.SellingCost";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetOnSaleProducts(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id
            where vg.IsShowOnHome = 1 order by NEWID() desc ";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse<ProductResponse>>>> GetFeatureProducts(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse<ProductResponse>>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id
            where vg.IsShowOnHome = 1 order by NEWID() desc ";
                res.Result = await _dapper.GetAllAsync<ProductResponse<ProductResponse>>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<HotDealsResponse>>> GetHotDeals(ProductRequest productRequest)
        {
            var res = new Response<IEnumerable<HotDealsResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,p.Description,'' DealEndsOn,'Hot Deal' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id
            where vg.IsShowOnHome = 1 order by NEWID() desc ";
                res.Result = await _dapper.GetAllAsync<HotDealsResponse>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }
        public async Task<IResponse<IEnumerable<AutoSuggest>>> GetAutoSuggetion()
        {
            var res = new Response<IEnumerable<AutoSuggest>>();
            try
            {
                string sqlQuery = @"Select c.CategoryName [Name],c.CategoryId Id,'C' [Type] from Category c(nolock)  
Union
Select p.[Name] [Name],p.Id Id,'P' [Type] from Products p(nolock) ";
                res.Result = await _dapper.GetAllAsync<AutoSuggest>(sqlQuery, null, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }


        public async Task<IResponse<IEnumerable<ProductResponse>>> GetProductByPID(ProductRequest<ProductFilter> productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"CREATE TABLE #temp (attributes varchar(max)) 
insert into #temp select * from  dbo.fn_SplitString(@Attributes,',')
  if((select count(*) from #temp)>0)
  begin
   with cte as ( Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p(nolock) 
            inner join VariantGroup vg(nolock) on vg.ProductId = p.Id 
			 inner join CategoryAttributeMapping cam(nolock) on cam.CategoryId=p.CategoryId
			  inner join AttributeInfo ai(nolock) on ai.AttributeId=cam.AttributeId
			 inner join #temp t on t.attributes = ai.AttributeValue and vg.id=ai.GroupId
            where p.Id = @ProductId
)select * from cte group by 	ProductID,VariantID,PublishedOn,Title,MRP,GroupID,ImagePath,Label,SellingCost,Stars
  end
  else
  begin
   Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from VariantGroup vg(nolock) inner join Products p(nolock) on p.Id = vg.ProductId where vg.ProductId = @ProductId
  end";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { productRequest.MoreFilters.ProductId, productRequest.MoreFilters.Attributes, Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }








    }
}
