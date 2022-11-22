using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Variants
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Product")]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Attribute")]
        public int AttributeId { get; set; }

        [Required(ErrorMessage = "Please Enter Attribute Value")]
        public string AttributeValue { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public int EntryBy { get; set; }
        public int ModifyBy { get; set; }
        public string EntryOn { get; set; }
        public string ModifyOn { get; set; }

    }
}
