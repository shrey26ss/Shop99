using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class ProductVariantGroup
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Product")]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Attribute")]
        public int AttributeId { get; set; }
        [Required(ErrorMessage = "AttributeValue")]
        public string AttributeValue { get; set; }
        public bool AllowFiltering { get; set; }
        public bool ShowOnProductPage { get; set; }

    }
    public class ProductVariant
    {
        public int Id { get; set; }
        public string HSN { get; set; }
        [Required(ErrorMessage = "MRP is required.")]
        public decimal MRP { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        public string GTIN { get; set; }
        public int ProductVariantsGroupId { get; set; }
    }
    public class VariantCombination
    {
        [Required(ErrorMessage = "Please add atleaste one variant")]
        public List<ProductVariant> ProductVariant { get; set; }
        [Required(ErrorMessage = "Please add atleaste one variant")]
        public List<ProductVariantGroup> ProductVariantGroups { get; set; }
    }
    public class ViewVariantCombinationModel : VariantCombination
    {
        public string CombinationId { get; set; }
        public List<AttributesDDL> Attributes { get; set; }
    }
}
