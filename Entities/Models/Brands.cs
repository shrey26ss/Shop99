using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Brands
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter CategoryId")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Please Enter Brand Name")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
