using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class VariantViewModel
    {
        public int ProductId { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }
        public List<ProductVariantGroup> ProductVariantGroups { get; set; }
    }
}
