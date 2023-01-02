using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Brands : BrandsDDL
    {
        //public string Description { get; set; }
        //public string Icon { get; set; }
        public bool IsPublished { get; set; }
    }
    public class BrandsDDL
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Brand Name")]
        public string Name { get; set; }
    }
    public class UpdateIspublishBrands
    {
        public int Id { get; set; }
        public bool IsPublish { get; set; }
    }
}
