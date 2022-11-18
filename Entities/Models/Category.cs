using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Please provide category name")]
        public string CategoryName { get; set; }
        public int ParentId { get; set; }
        public bool IsPublish { get; set; }
        public string Icon { get; set; }
    }
}
