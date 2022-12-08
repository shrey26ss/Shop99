using Entities.Models;
using Infrastructure.Interface;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class ProductViewModel : Products
    {
        public List<CategoryDDL> Categories { get; set; }
        public List<BrandsDDL> Brands { get; set; }
    }
    public class VariantViewModel
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public List<AttributeInfo> ProductVariants { get; set; }
        public List<VariantGroup> ProductVariantGroups { get; set; }
        public List<AttributesDDL> AttributesDDLs { get; set; }
    }
    public class ProductSection
    {
        public int TabID { get; set; }
        public IResponse<IEnumerable<ProductResponse>> ProductsData { get; set; }
    }
}
