using Entities.Models;
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
        public List<ProductVariant> ProductVariants { get; set; }
        public List<ProductVariantGroup> ProductVariantGroups { get; set; }
        public List<AttributesDDL> AttributesDDLs { get; set; }
    }

}
