using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Products : ProductShippingDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Please Enter Title")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Brand")]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int EntryBy { get; set; }   
        public int ModifyBy { get; set; }   
        public string EntryOn { get; set; }   
        public string ModifyOn { get; set; }   
        public bool IsPublished { get; set; }
        public string Specification { get; set; }
        public string ShortDescription { get; set; }
    }
    public class ProductShippingDetail
    {
        public int ShippingDetailId { get; set; }
        public int ProductId { get; set; }
        [Required (ErrorMessage ="Please Enter Charges")]
        public decimal Charges { get; set; }
        public decimal FreeOnAmount { get; set; }
        public bool IsFlat { get; set; }
    }
    public class UpdateIsPublishProduct
    {
        public int ID { get; set; }
        public bool IsPublish { get; set; }
    }
}
