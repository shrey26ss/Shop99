using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Brand")]
        public int BrandId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Category")]
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
        public int EntryBy { get; set; }   
        public int ModifyBy { get; set; }   
        public string EntryOn { get; set; }   
        public string ModifyOn { get; set; }   
    }
}
