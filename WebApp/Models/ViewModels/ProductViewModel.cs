using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class ProductViewModel
    {
        public Products Products { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }
    }
}
