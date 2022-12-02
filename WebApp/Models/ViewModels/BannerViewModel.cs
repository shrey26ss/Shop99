using Dapper;
using Entities.Models;
using FluentMigrator.Infrastructure;
//using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models.ViewModels
{
    public class TopBannerViewModel : TopBanner
    {
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select file.")]
        public IFormFile File { get; set; }
    }
    public class TopLowerBannerViewModel : TopLowerBanner
    {
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select file.")]
        public IFormFile File { get; set; }
    }
    public class TopBannerDashBoard
    {
        public List<TopBanner> topBanner { get; set; }
        public List<TopLowerBanner> topLowBanner { get; set; }
    }
}
