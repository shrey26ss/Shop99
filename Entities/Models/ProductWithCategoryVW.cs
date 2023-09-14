using System.Collections.Generic;

namespace Entities.Models
{
    public class ProductWithCategoryVW
    {
        public List<ProductsColumn> Products { get; set; }
        public List<Category> Category { get; set; }
    }
}
