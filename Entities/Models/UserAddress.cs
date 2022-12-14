using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Full Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not A Valid Mobile Number")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Enter Pin Code")]
        [DataType(DataType.PostalCode)] 
        [RegularExpression(@"^[1-9]{1}[0-9]{2}\s{0,1}[0-9]{3}$", ErrorMessage = "Not A Valid Pin Code Number")]
        public string Pincode { get; set; }
        [Required(ErrorMessage = "Please Enter House Number")]
        public string HouseNo { get; set; }
        [Required(ErrorMessage = "Please Enter Your Area")]
        public string Area { get; set; }
        [Required(ErrorMessage = "Please Enter Nearest Landmark")]
        public string Landmark { get; set; }
        [Required(ErrorMessage = "Please Enter Your City")]
        public string TownCity { get; set; }
        [Required(ErrorMessage = "Please Select State")]
        public int StateID { get; set; }
        [Required(ErrorMessage = "Please Select Address Type")]
        public int AddressTypeID { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
        public bool IsDefault { get; set; }
        public string StateName { get; set; }
        public string AddressType { get; set; }
    }
}
