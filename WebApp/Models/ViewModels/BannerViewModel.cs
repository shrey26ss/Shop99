using Dapper;
using Entities.Models;
using Microsoft.AspNetCore.Http;

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
}
