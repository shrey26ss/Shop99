using Service.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.ViewModels
{
    public class LoginViewModel
    {
        public string MobileNo { get; set; }
        //[EmailAddress]
        //public string EmailId { get; set; }
        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        [NotMapped]
        public bool IsTwoFactorEnabled { get; set; }
        [Display(Name = "Authentication Pin")]
        public string GAuthPin { get; set; }
        public string OTP { get; set; }
        public string StatusCode { get; set; }

    }
}
