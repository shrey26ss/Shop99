using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Attributes
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please provide attribute name")]
        public string Name { get; set; }
        public string Value { get; set; }        
    }
}
