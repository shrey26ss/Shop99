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
        [Required(ErrorMessage = "Enter Banner Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Enter Banner Sub Title")]
        public string Subtitle { get; set; }
        [Required(ErrorMessage = "Enter Back Link Text")]
        [Display(Name = "Back Link Text")]
        public string BackLinkText { get; set; }

        [Required(ErrorMessage = "Enter Back Link URL")]
        [Display(Name = "Back Link URL")]
        public string BackLinkURL { get; set; }
    }
}
