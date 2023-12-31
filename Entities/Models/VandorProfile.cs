﻿using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class VendorProfile 
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Shop Name")]
        [Display(Name ="Shop Name")]
        public string ShopName { get; set; }
        [Required(ErrorMessage = "Please Enter GST Number")]
        [Display(Name ="GST Number")]
        public string GSTNumber { get; set; }
      
        public string TIN { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please Select State")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Address { get; set; }
        [Display(Name ="Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactNo { get; set; }
    }
    public class VendorProfileList : ApplicationUser
    {
        public string State { get; set; }
        public string ShopName { get; set; }
        public string GSTNumber { get; set; }
        public string TIN { get; set; }
        public int VendorId { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public bool IsApproved { get; set; }
    }
    public class VendorProfileRequest
    {
        public int UserId { get; set; }
        public int VendorId { get; set; }
    }
    public class ValidateVendor
    {
        public bool IsOnboard { get; set; }
        public bool IsApproved { get; set; }
        public string ResponseText { get; set; }
    }
}
