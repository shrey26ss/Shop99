using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Attributes : AttributesDDL
    {
    }
    public class AttributesDDL
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide attribute name")]
        public string Name { get; set; }
    }
}
