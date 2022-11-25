using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Product")]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Attribute")]
        public int AttributeId { get; set; }
        public int ProductVariantsGroupId { get; set; }
        public bool AllowFiltering { get; set; }
        

    }
    public class ProductVariantGroup
    {
        public int Id { get; set; }
        public string AttributeValue { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public bool ShowOnProductPage { get; set; }
        public string HSN { get; set; }
        public string GTIN { get; set; }
    }
    public class VariantCombination
    {
        public List<ProductVariant> ProductVariants { get; set; }
        public List<ProductVariantGroup> ProductVariantGroups { get; set; }
    }
}
