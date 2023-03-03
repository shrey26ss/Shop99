using AppUtility.Helper;
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
using System.Threading.Tasks;

namespace Service.Homepage
{
    public class HomepageService : IHomepageService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        private readonly INotifyService _notify;
        public HomepageService(IDapperRepository dapper, ILogger<DapperRepository> logger, INotifyService notify)
        {
            _notify = notify;
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IResponse<JDataTableResponse<ProductResponse>>> GetProductByCategory(ProductRequest<CategorFilter> productRequest)
        {
            var res = new Response<JDataTableResponse<ProductResponse>>
            {
                Result= new JDataTableResponse<ProductResponse>
                {
                    Data = new List<ProductResponse>()
                }
            };
            try
            {
                productRequest.Top = productRequest.Top < 1 ? 10 : productRequest.Top;
                //productRequest.Start = (productRequest.Start-1)*productRequest.Top;
                //productRequest.Start =productRequest.Start < 0 ? 0 : productRequest.Start;
                var result = await _dapper.GetMultipleAsync<ProductResponse, JDataTableResponse>("proc_GetProductByCategory", new
                {
                    productRequest.Start,
                    length = productRequest.Top,
                    productRequest.OrderBy,
                    productRequest.MoreFilters.CategoryId,
                    productRequest.MoreFilters.Attributes,
                    productRequest.UserID,
                }, CommandType.StoredProcedure);
                var products = (List<ProductResponse>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var jdataTableResponse = (List<JDataTableResponse>)result.GetType().GetProperty("Table2").GetValue(result, null);
                res.Result.recordsTotal= jdataTableResponse.FirstOrDefault()?.recordsTotal ?? 0;
                res.Result.start = productRequest.Start;
                res.Result.OrderBy = productRequest.OrderBy;
                res.Result.draw = productRequest.Top;
                res.Result.Data = products;
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
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,vg.Rating  from Products p(nolock) 
            inner join VariantGroup vg(nolock) on vg.ProductId = p.Id
 left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1
            where p.IsPublished = 1 and vg.IsShowOnHome = 1 and p.ispublished = 1 and vg.ispublished = 1 order by NEWID() desc ";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetNewArrivals(ProductRequest<int> productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select  top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn)       
                                            PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,
                                           vg.Rating 
                                    from    Products p (nolock)
                                            inner join VariantGroup vg on vg.ProductId = p.Id 
 left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1
                                    where   vg.IsShowOnHome=1 and p.ispublished = 1 and vg.ispublished = 1
                                            and vg.AdminApproveStatus = 3
                                    order by vg.PublishedOn desc ";
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
                string sqlQuery = @"Select  top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,
                                            vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,vg.Rating  
                                    from    Products p (nolock)
                                            inner join VariantGroup vg(nolock) on vg.ProductId = p.Id 
 left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1

                                    where vg.IsFeatured=1 and p.ispublished = 1 and vg.ispublished = 1";
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
                string sqlQuery = @"Select  top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,
                                            vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,vg.Rating  
                                    from    Products p (nolock)
                                            inner join VariantGroup vg on vg.ProductId = p.Id and p.ispublished = 1 and vg.ispublished = 1 and 
vg.AdminApproveStatus = 3 
 left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1
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
                string sqlQuery = @"Select  top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,
                                            vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,vg.Rating 
                                    from    Products p (nolock)
                                            inner join VariantGroup vg on vg.ProductId = p.Id
 left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1
                                    where   vg.IsShowOnHome = 1 and vg.AdminApproveStatus = 3 and p.ispublished = 1 and vg.ispublished = 1 order by NEWID() desc ";
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
                string sqlQuery = @"Select  top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,
                                            vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,vg.Rating  
                                    from    Products p (nolock)
                                            inner join VariantGroup vg(nolock) on vg.ProductId = p.Id
  left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1
                                    where vg.IsShowOnHome = 1 and vg.AdminApproveStatus = 3 and p.ispublished = 1 and vg.ispublished = 1 order by NEWID() desc ";
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
                string sqlQuery = @"Select  top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,
                                            vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,p.Description,p.ShortDescription,'' DealEndsOn,'Hot Deal' [Label],
                                            Format(vg.SellingCost-vg.SellingCost*isnull(o.AtRate,0)/100,'N2') as SellingCost,vg.Rating  
                                    from    Products p (nolock)
                                            inner join VariantGroup vg(nolock) on vg.ProductId = p.Id
left join Offers o on vg.OfferID=o.OfferId and o.IsActive=1
                                    where   vg.IsShowOnHome = 1 and p.ispublished = 1 and vg.ispublished = 1 order by NEWID() desc ";
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
        public async Task<IResponse<IEnumerable<AutoSuggest>>> GetAutoSuggetion(string searchText = "", int Top = 0)
        {
            var res = new Response<IEnumerable<AutoSuggest>>();
            try
            {
                string sqlQuery = @"if @Top = 0
Select Distinct Top(50) c.CategoryName [Name],c.CategoryId Id,'C' [Type] from Category c(nolock)  where c.IsPublish = 1 and c.[CategoryName] Like '%'+ @searchText +'%'
Union
Select Distinct Top(50) p.[Name] [Name],p.Id Id,'P' [Type] from Products p(nolock) where p.IsPublished = 1 and p.Name Like '%'+ @searchText +'%'
Union 
Select Distinct Top(50) p.[Title] [Name],p.Id Id,'V' [Type] from VariantGroup p(nolock) where p.IsPublished = 1 and p.Title Like '%'+ @searchText +'%'
UNION 
Select DISTINCT TOP(50) b.[Name] [Name],b.Id Id,'B' [Type] from Brands b(nolock) where b.IsPublished = 1 and b.[Name] Like '%'+ @searchText +'%'
else
Select Distinct Top(@Top) c.CategoryName [Name],c.CategoryId Id,'C' [Type] from Category c(nolock)  where c.IsPublish = 1 and c.[CategoryName] Like '%'+ @searchText +'%'
Union
Select Distinct Top(@Top) p.[Name] [Name],p.Id Id,'P' [Type] from Products p(nolock) where p.IsPublished = 1 and p.Name Like '%'+ @searchText +'%'
Union
Select Distinct Top(@Top) p.[Title] [Name],p.Id Id,'V' [Type] from VariantGroup p(nolock) where p.IsPublished = 1 and p.Title Like '%'+ @searchText +'%'
Union
Select DISTINCT TOP(@Top) b.[Name] [Name],b.Id Id,'B' [Type] from Brands b(nolock) where b.IsPublished = 1 and b.[Name] Like '%'+ @searchText +'%'";
                res.Result = await _dapper.GetAllAsync<AutoSuggest>(sqlQuery, new { searchText = searchText ?? "", Top }, CommandType.Text);
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public async Task<IResponse<JDataTableResponse<ProductResponse>>> GetProductByBrandID(ProductRequest<BrandFilter> productRequest)
        {
            var res = new Response<JDataTableResponse<ProductResponse>>
            {
                Result = new JDataTableResponse<ProductResponse>
                {
                    Data = new List<ProductResponse>()
                }
            };
            try
            {
                productRequest.Top = productRequest.Top < 1 ? 10 : productRequest.Top;
                var result = await _dapper.GetMultipleAsync<ProductResponse, JDataTableResponse>("proc_GetProductFromAllScenario", new
                {
                    productRequest.Start,
                    length = productRequest.Top,
                    productRequest.OrderBy,
                    productRequest.MoreFilters.Id,
                    productRequest.MoreFilters.Attributes,
                    productRequest.CalledFrom
                }, CommandType.StoredProcedure);
                var products = (List<ProductResponse>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var jdataTableResponse = (List<JDataTableResponse>)result.GetType().GetProperty("Table2").GetValue(result, null);
                res.Result.recordsTotal = jdataTableResponse.FirstOrDefault()?.recordsTotal ?? 0;
                res.Result.start = productRequest.Start;
                res.Result.OrderBy = productRequest.OrderBy;
                res.Result.draw = productRequest.Top;
                res.Result.Data = products;
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }
        public async Task<IResponse<JDataTableResponse<ProductResponse>>> GetProductByPID(ProductRequest<ProductFilter> productRequest)
        {
            var res = new Response<JDataTableResponse<ProductResponse>>
            {
                Result = new JDataTableResponse<ProductResponse>
                {
                    Data = new List<ProductResponse>()
                }
            };
            try
            {
                productRequest.Top = productRequest.Top < 1 ? 10 : productRequest.Top;
                var result = await _dapper.GetMultipleAsync<ProductResponse, JDataTableResponse>("proc_GetProductByProdID", new
                {
                    productRequest.Start,
                    length = productRequest.Top,
                    productRequest.OrderBy,
                    productRequest.MoreFilters.ProductId,
                    productRequest.MoreFilters.Attributes,
                }, CommandType.StoredProcedure);
                var products = (List<ProductResponse>)result.GetType().GetProperty("Table1").GetValue(result, null);
                var jdataTableResponse = (List<JDataTableResponse>)result.GetType().GetProperty("Table2").GetValue(result, null);
                res.Result.recordsTotal = jdataTableResponse.FirstOrDefault()?.recordsTotal ?? 0;
                res.Result.start = productRequest.Start;
                res.Result.OrderBy = productRequest.OrderBy;
                res.Result.draw = productRequest.Top;
                res.Result.Data = products;
                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = nameof(ResponseStatus.Success);
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }
        public async Task<IResponse> AddNewsLetter(RequestBase<NewsLetter> request)
        {
            var res = new Response();
            try
            {
                string sqlQuery = "";
                int i = -5;
                
                 sqlQuery = @"if not  exists (select ID from NewsLetter where Email=@Email)
                              begin
                              insert into NewsLetter(Name,Email,CeratedOn)values(@Name,@Email,getdate())
                              end";
                i = await _dapper.ExecuteAsync(sqlQuery, new
                {
                    request.Data.Name,
                    request.Data.Email,
                }, CommandType.Text);
                var description = Utility.O.GetErrorDescription(i);
                if (i > 0 && i < 10)
                {
                    #region Send Email
                    await _notify.SaveSMSEmailWhatsappNotification(new SMSEmailWhatsappNotification() { FormatID = MessageFormat.NewsLetter, IsEmail = true,EmailID = request.Data.Email},0);
                    #endregion
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
                else
                {
                    res.StatusCode = ResponseStatus.Failed;
                    res.ResponseText = "Email Allready Exists Try Another Email.!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }

    }
}
