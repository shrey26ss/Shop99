using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class CategoryBrandMapping : BrandMapping
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class BrandMapping
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
