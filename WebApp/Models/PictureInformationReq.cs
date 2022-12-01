using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WebApp.Models
{
    public class PictureInformationReq
    {
        public string Color { get; set; }
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Please Enter Display Order")]
        [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "Please Enter Numeric Value")]
        public int DisplayOrder { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage ="Please Enter Alt")]
        public string Alt { get; set; }
        public IFormFile file { get; set; }
    }
}
