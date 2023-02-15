using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class TopLowerBanner
    {
        public int Id { get; set; }
        public string BannerPath { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [Display(Name = "Back Link Text")]
        public string BackLinkText { get; set; }
        [Display(Name = "Back Link URL")]
        public string BackLinkURL { get; set; }
    }
}
