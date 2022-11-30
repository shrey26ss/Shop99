

using Infrastructure.Interface;

namespace Service.Models
{
    public class ProductResponse : IProductResponse
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

    public class ProductResponse<T> : ProductResponse, IProductResponse<T>
    {
        T IProductResponse<T>.MoreDetails { get ; set; }
    }
    public class ProductRequest : IProductRequest
    {
        public string OrderBy { get; set; }
        public int Top { get; set; }
    }
    public class ProductRequest<T> : ProductRequest, IProductRequest<T>
    {
        T IProductRequest<T>.MoreFilters { get; set; }
    }
}
