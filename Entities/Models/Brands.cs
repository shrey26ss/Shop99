using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Entity
{
    public class Brands : BrandsDDL
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }
    public class BrandsDDL
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Brand Name")]
        public string Name { get; set; }
    }
}
