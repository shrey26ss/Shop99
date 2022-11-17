using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentId { get; set; }
        public bool IsPublish { get; set; }
        public string Icon { get; set; }
    }
}
