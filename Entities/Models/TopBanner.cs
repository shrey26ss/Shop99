using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class TopBanner
    {
        public int Id { get; set; }
        public string BannerPath { get; set; }        
        public string Title { get; set; }        
        public string Subtitle { get; set; }
        public string BackLinkText { get; set; }
        public string BackLinkURL { get; set; }
    }
}
