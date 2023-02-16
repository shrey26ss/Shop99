using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels
{
    public class WebsiteinfoViewModel: WebsiteinfoModel
    {
        [Required(ErrorMessage = "Please select file.")]
        public IFormFile Whitelogofile { get; set; }
        public IFormFile Coloredlogofile { get; set; }
    }
}
