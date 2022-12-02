using Dapper;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class TopBannerViewModel : TopBanner
    {
        [NotMapped]
        public IFormFile File { get; set; }
    }
    public class TopLowerBannerViewModel : TopLowerBanner
    {
        [NotMapped]
        public IFormFile File { get; set; }
    }
    public class TopBannerDashBoard
    {
        public List<TopBanner> topBanner { get; set; }
        public List<TopLowerBanner> topLowBanner { get; set; }
    }
}
