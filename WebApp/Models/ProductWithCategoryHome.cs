using Entities.Models;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class ProductWithCategoryHome
    {
        public Category category { get; set; }
        public List<ProductsColumn> products { get; set; }
    }
}
