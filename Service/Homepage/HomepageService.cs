using Data;

using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<IResponse<List<IProductResponse>>> GetProductByCategory(IProductRequest<int> productRequest)
        {
            throw new NotImplementedException();
            //            List<ProductResponse> res = new List<ProductResponse>();
            //            try
            //            {
            //                int i = -5;
            //                string sqlQuery = @"Select vg.ProductId ProductID,vg.Id VariantID,vg.PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            //inner join VariantGroup vg on vg.ProductId = p.Id 
            //where p.CategoryId = @MoreFilters and vg.IsShowOnHome=1 ";
            //                i = await _dapper.GetAllAsync(sqlQuery, new
            //                {
            //                    productRequest.MoreFilters
            //                }, CommandType.Text);
            //                if (i > -1 && i < 100)
            //                {
            //                    res.StatusCode = ResponseStatus.Success;
            //                    res.ResponseText = ResponseStatus.Success.ToString();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //            }

            //            return res;
        }

        public Task<IResponse<List<IProductResponse>>> GetRandomProduct(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<IProductResponse>>> GetNewArrivals(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<IProductResponse>>> GetFeaturedProduct(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<IProductResponse>>> GetBestSellerProduct(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<IProductResponse>>> GetOnSaleProducts(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<IProductResponse<IHotDealsResponse>>>> GetHotDeals(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }
    }
}
