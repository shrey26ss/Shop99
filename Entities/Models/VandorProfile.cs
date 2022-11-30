using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entity
{
    public class VendorProfile
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Shop Name")]
        public string ShopName { get; set; }
        [Required(ErrorMessage = "Please Enter GST Number")]
        public string GSTNumber { get; set; }
        [Required(ErrorMessage = "Please Enter Your TIN")]
        public string TIN { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
    }
}
