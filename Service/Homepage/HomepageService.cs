using Data;
using Entities.Enums;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<IResponse<IEnumerable<IProductResponse>>> GetProductByCategory(IProductRequest<int> productRequest)
        {
            IResponse<IEnumerable<IProductResponse>> res = new Response<IEnumerable<IProductResponse>>();
            try
            {
                int i = -5;
                string sqlQuery = @"Select vg.ProductId ProductID,vg.Id VariantID,vg.PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
            where p.CategoryId = @MoreFilters and vg.IsShowOnHome=1 ";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { productRequest.MoreFilters }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public Task<IResponse<IEnumerable<IProductResponse>>> GetRandomProduct(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<IProductResponse>>> GetNewArrivals(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<IProductResponse>>> GetFeaturedProduct(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<IProductResponse>>> GetBestSellerProduct(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<IProductResponse>>> GetOnSaleProducts(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<IProductResponse<IHotDealsResponse>>>> GetHotDeals(IProductRequest productRequest)
        {
            throw new NotImplementedException();
        }
    }
}
