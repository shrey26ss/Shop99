using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class MenuItem : Category
    {
        public IEnumerable<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
    public class Menu
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
     
        public Menu(int CategoryId, string CategoryName, int? ParentId)
        {
            this.CategoryId = CategoryId;
            this.CategoryName = CategoryName;
            this.ParentId = ParentId;
        }
    }
}
