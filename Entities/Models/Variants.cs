using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class VariantGroup
    {
        public int Id { get; set; }
        public string HSN { get; set; }
        [Required(ErrorMessage = "MRP is required.")]
        public decimal MRP { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }
        public string GTIN { get; set; }
        public bool IsShowOnHome { get; set; }
        public string Title { get; set; }
        public decimal SellingCost { get; set; }
        public bool IsFeatured { get; set; }
    }
    public class AttributeInfo
    {
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public bool AllowFiltering { get; set; }
        public int GroupId { get; set; }    

    }
    public class PictureInformation
    {
        public int GroupId { get; set; }
        public string Color { get; set; }    
        public int DisplayOrder { get; set; }    
        public string Title { get; set; }    
        public string Alt { get; set; }    
        public string ImagePath { get; set; }
        public string ImgVariant { get; set; }
    }
    public class VariantCombination
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please add atleaste one variant")]
        public List<AttributeInfo> AttributeInfo { get; set; }
        [Required(ErrorMessage = "Please add atleaste one variant")]
        public List<VariantGroup> GroupInfo { get; set; }
        public List<PictureInformation> PictureInfo { get; set; }
    }
    public class ViewVariantCombinationModel : VariantCombination
    {
        public string CombinationId { get; set; }
        public List<AttributesDDL> Attributes { get; set; }
    }
}
