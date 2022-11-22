using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Please Enter Category Name")]
        public string CategoryName { get; set; }
        public int ParentId { get; set; }
        public bool IsPublish { get; set; }
        public string Icon { get; set; }
    }
}
