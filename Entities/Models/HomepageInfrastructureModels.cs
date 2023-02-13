using Entities.Enums;

namespace Entities.Models
{
    public class ProductResponse
    {
        public string ImagePath { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public string Title { get; set; }
        public decimal Rating { get; set; }
        public string Label { get; set; }
        public string PublishedOn { get; set; }
        public int ProductID { get; set; }
        public int VariantID { get; set; }
        public int WishListID { get; set; }
    }

    public class ProductResponse<T> : ProductResponse
    {
        public T MoreDetails { get ; set; }
        
    }
    public class ProductRequest 
    {
        public SortingOption OrderBy { get; set; }
        public int Top { get; set; }
        public int UserID { get; set; }
        public int Start { get; set; }
        public char CalledFrom { get; set; } = 'A';
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
        public string ShortDescription { get; set; }
        public string DealEndsOn { get; set; }
    }
    public class AutoSuggest
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }
    public class ProductFilter
    {
        public int ProductId { get; set; }
        public string Attributes { get; set; }
    }
    public class BrandFilter
    {
        public int BrandId { get; set; }
        public string Attributes { get; set; }
    }
}
