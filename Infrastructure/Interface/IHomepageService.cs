using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IHomepageService
    {
        Task<IResponse<List<IProductResponse>>> GetProductByCategory(IProductRequest<int> productRequest);
        Task<IResponse<List<IProductResponse>>> GetRandomProduct(IProductRequest productRequest);
        Task<IResponse<List<IProductResponse>>> GetNewArrivals(IProductRequest productRequest);
        Task<IResponse<List<IProductResponse>>> GetFeaturedProduct(IProductRequest productRequest);
        Task<IResponse<List<IProductResponse>>> GetBestSellerProduct(IProductRequest productRequest);
        Task<IResponse<List<IProductResponse>>> GetOnSaleProducts(IProductRequest productRequest);
        Task<IResponse<List<IProductResponse<IHotDealsResponse>>>> GetHotDeals(IProductRequest productRequest);
        //IProductResponse GetTopBanners(IProductRequest productRequest);
        //IProductResponse GetTopLowerBanners();
    }

    public interface IProductResponse
    {
        public string ImagePath { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public string Title { get; set; }
        public int Stars { get; set; }
        public string Label { get; set; }
        public string PublishedOn { get; set; }
        public int ProductID { get; set; }
        public int VariantID { get; set; }
    }

    public interface IProductResponse<T> : IProductResponse
    {
        T MoreDetails { get; set; }
    }
    public interface IHotDealsResponse
    {
        string Description { get; set; }
        string DealEndsOn { get; set; }
    }
    public interface IProductRequest
    {
        public string OrderBy { get; set; }
        public int Top { get; set; }
    }
    public interface IProductRequest<T> : IProductRequest
    {
        T MoreFilters { get; set; }
    }
}
