using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class VariantGroup
    {
        public int Id { get; set; }
        [MaxLength(12, ErrorMessage = "Allowed Only 12 Character")]
        public string HSN { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal? MRP { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int? Quantity { get; set; }
        public int? ReturnInDays { get; set; }

        [MaxLength(30, ErrorMessage = "Allowed Only 30 Character")]
        public string GTIN { get; set; }
        public bool? IsShowOnHome { get; set; }
        [Required(ErrorMessage = "Please Enter Title")]
        [MaxLength(320, ErrorMessage = "Allowed Only 320 Character")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please Enter Selling Cost")]
        public decimal? SellingCost { get; set; }
        public bool? IsFeatured { get; set; }
    }
    public class AttributeInfo
    {
        public int? AttributeId { get; set; }
        [MaxLength(120, ErrorMessage = "Allowed Only 120 Character")]
        public string AttributeValue { get; set; }
        public bool? AllowFiltering { get; set; }
        public int? GroupId { get; set; }

    }
    public class PictureInformation
    {
        public int? Id { get; set; }
        public int? GroupId { get; set; }
        public string Color { get; set; }
        public int? DisplayOrder { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public string ImagePath { get; set; }
        public string ImgVariant { get; set; }
    }
    public class VariantCombination
    {
        [Required(ErrorMessage = "Product Id is mendetory")]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        [Required, MinLength(1, ErrorMessage = "Please add atleaste one Attribute")]
        public List<AttributeInfo> AttributeInfo { get; set; }
        [Required, MinLength(1, ErrorMessage = "Please add atleaste one variant")]
        public List<VariantGroup> GroupInfo { get; set; }
        public List<PictureInformation> PictureInfo { get; set; }
    }
    public class ViewVariantCombinationModel : VariantCombination
    {
        public string CombinationId { get; set; }
        public List<AttributesDDL> Attributes { get; set; }
        public int VariantId { get; set; }
        public string VariantColor { get; set; }
        public string ImgAlt { get; set; }
    }

    public class VariantIdByAttributesRequest
    {
        public int VariantId { get; set; }
        public string Attributes { get; set; }
        public string Color { get; set; }
    }

    public class VariantIdByAttributesResponse
    {
        public int VariantId { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingPrice { get; set; }
    }
    public class VariantQuantity
    {
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public bool IsReduce { get; set; }
        public string Remark { get; set; }

    }
    public class VariantDetailsByAttributesResponse
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
    }
    public class DeleteVariantReq
    {
        public int VariantId { get; set; }
        public int ImgId { get; set; }
        public string ImgPath { get; set; }

    }
}
