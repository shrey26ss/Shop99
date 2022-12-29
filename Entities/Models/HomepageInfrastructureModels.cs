namespace Entities.Models
{
    public class ProductResponse
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

    public class ProductResponse<T> : ProductResponse
    {
        public T MoreDetails { get ; set; }
        
    }
    public class ProductRequest 
    {
        public string OrderBy { get; set; }
        public int Top { get; set; }
    }
    public class ProductRequest<T> : ProductRequest
    {
        public T MoreFilters { get; set; }
    }

    public class CategorFilter
    {
        public int CategoryId { get; set; }
        public string Attributes { get; set; }
    }
    public class HotDealsResponse: ProductResponse
    {
        public string Description { get; set; }
        public string DealEndsOn { get; set; }
    }
    public class AutoSuggest
    {
        public string Product { get; set; }
        public int ProductId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
    }
   
}
