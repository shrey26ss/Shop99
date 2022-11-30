using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace Entities.Entity
{
    public class Register : Response
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select role")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [StringLength(200)]
        [RegularExpression(@"[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are Allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Mobile")]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(0|91)?[6-9][0-9]{9}", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Invalid Email Address")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
    }
}
