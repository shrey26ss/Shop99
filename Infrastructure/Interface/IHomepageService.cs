using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IHomepageService
    {
        Task<IResponse<IEnumerable<ProductResponse>>> GetProductByCategory(ProductRequest<int> productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetRandomProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetNewArrivals(ProductRequest<int> productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetFeaturedProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetBestSellerProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetOnSaleProducts(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse<HotDealsResponse>>>> GetHotDeals(ProductRequest productRequest);       
    }
}
