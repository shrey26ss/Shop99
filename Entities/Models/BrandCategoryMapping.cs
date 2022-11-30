using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entity
{
    public class BrandCategoryMapping
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please provide Brand")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide Brand")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Please provide Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide Category")]
        public int CategoryId { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}
