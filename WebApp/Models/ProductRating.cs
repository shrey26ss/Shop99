using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class ProductRating
    {
        public int VariantID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public string Images { get; set; }
        public List<IFormFile> file { get; set; }
    }
    public class ProductRatingImg
    {
        public IFormFile file { get; set; }
    }
    public class ProductWiseRating: ProductRating
    {
        public int heighstar { get; set; }
        public int ID { get; set; }
        public string CreatedOn { get; set; }
        public string Name { get; set; }
        public int Top1 { get; set; }
        public int Top2 { get; set; }
        public int Top3 { get; set; }
        public int Top4 { get; set; }
        public int Top5 { get; set; }
    }
}
