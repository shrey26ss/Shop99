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

        public Task<IResponse<IEnumerable<IProductResponse>>> GetProductByCategory(IProductRequest<int> productRequest)
        {
            throw new NotImplementedException();
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
