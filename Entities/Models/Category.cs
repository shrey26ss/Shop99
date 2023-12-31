﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Category : CategoryDDL
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public decimal TAXRate { get; set; }
        public bool IsPublish { get; set; }
        public string Icon { get; set; }
        public bool? IsVendorGrouped { get; set; }
    }
    public class CategoryDDL
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        [Required(ErrorMessage = "Please Enter Category Name")]
        public string CategoryName { get; set; }
    }
    public class CategoryIsPublishUpdate 
    {
        public int ParentId { get; set; }
        public bool IsPublish { get; set; }
    }
    public class TopCategoryRequest
    {
        public int Top { get; set; } = 5;
    }

}
