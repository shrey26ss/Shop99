using Data;
using Entities.Enums;
using Entities.Models;
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

        public async Task<IResponse<IEnumerable<ProductResponse>>> GetProductByCategory(ProductRequest<int> productRequest)
        {
            var res = new Response<IEnumerable<ProductResponse>>();
            try
            {
                string sqlQuery = @"Select top (@Top) vg.ProductId ProductID,vg.Id VariantID,dbo.fn_DT_FullFormat(vg.PublishedOn) PublishedOn,vg.Title,vg.MRP,vg.Id GroupID,vg.Thumbnail ImagePath,'New' [Label],vg.SellingCost,4 Stars from Products p 
            inner join VariantGroup vg on vg.ProductId = p.Id 
            where p.CategoryId = @MoreFilters and vg.IsShowOnHome=1 ";
                res.Result = await _dapper.GetAllAsync<ProductResponse>(sqlQuery, new { productRequest.MoreFilters, Top = productRequest.Top < 1 ? 10 : productRequest.Top }, CommandType.Text);

                res.StatusCode = ResponseStatus.Success;
                res.ResponseText = "";
            }
            catch (Exception ex)
            {
                res.ResponseText = ex.Message;
            }
            return res;
        }

        public Task<IResponse<IEnumerable<ProductResponse>>> GetRandomProduct(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<ProductResponse>>> GetNewArrivals(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<ProductResponse>>> GetFeaturedProduct(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<ProductResponse>>> GetBestSellerProduct(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<ProductResponse>>> GetOnSaleProducts(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<IEnumerable<ProductResponse<HotDealsResponse>>>> GetHotDeals(ProductRequest productRequest)
        {
            throw new NotImplementedException();
        }
    }
}
