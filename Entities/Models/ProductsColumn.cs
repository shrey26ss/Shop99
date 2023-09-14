using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ProductsColumn: Products
    {
        public string Thumbnail { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public int VariantId { get; set; }
    }
}
