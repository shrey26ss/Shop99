using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class ProductRating
    {
        public int VariantID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public int Reting { get; set; }
        public string Review { get; set; }
        public string Images { get; set; }
        public IFormFile file { get; set; }
    }
    public class ProductRatingImg
    {
        public IFormFile file { get; set; }
    }
}
