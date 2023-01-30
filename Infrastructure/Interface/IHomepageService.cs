using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IHomepageService
    {
        Task<IResponse<JDataTableResponse<ProductResponse>>> GetProductByCategory(ProductRequest<CategorFilter> productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetProductByPID(ProductRequest<ProductFilter> productRequest);
        Task<IResponse<JDataTableResponse<ProductResponse>>> GetProductByBrandID(ProductRequest<BrandFilter> productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetRandomProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetNewArrivals(ProductRequest<int> productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetFeaturedProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetBestSellerProduct(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse>>> GetOnSaleProducts(ProductRequest productRequest);
        Task<IResponse<IEnumerable<ProductResponse<ProductResponse>>>> GetFeatureProducts(ProductRequest productRequest);
        Task<IResponse<IEnumerable<HotDealsResponse>>> GetHotDeals(ProductRequest productRequest);
        Task<IResponse<IEnumerable<AutoSuggest>>> GetAutoSuggetion(string searchText = "", int Top = 0);
        Task<IResponse> AddNewsLetter(RequestBase<NewsLetter> request);

    }
}
