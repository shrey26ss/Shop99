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
        public bool IsPublish { get; set; }
        [Required(ErrorMessage = "Please Select Icon")]
        public string Icon { get; set; }
        public bool? IsVendorGrouped { get; set; }
    }
    public class CategoryDDL
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please Enter Category Name")]
        public string CategoryName { get; set; }
    }
}
