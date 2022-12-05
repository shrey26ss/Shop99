using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels
{
    public class TopBannerViewModel : TopBanner
    {
        [Required(ErrorMessage = "Please select file.")]
        public IFormFile File { get; set; }
    }
    public class TopLowerBannerViewModel : TopLowerBanner
    {
        [Required(ErrorMessage = "Please select file.")]
        public IFormFile File { get; set; }
    }
    public class TopBannerDashBoard
    {
        public List<TopBanner> topBanner { get; set; }
        public List<TopLowerBanner> topLowBanner { get; set; }
    }
}
