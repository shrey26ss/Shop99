using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ProductVariantAttributeDetails
    {
        public int Id { get; set; } 
        public int ProductId { get; set; } 
        public string HSN { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingCost { get; set; }
        public int Quantity { get; set; }
        public string GTIN { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsShowOnHome { get; set; }
        public bool IsPublished { get; set; }
        public int EntryBy { get; set; }
        public StatusType AdminApproveStatus { get; set; }
    }
    public class AttrinutesViewModel
    {
        public int Id { get; set; }
        public StatusType statusid { get; set; }
    }
}
